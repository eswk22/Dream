using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Automation;
using Application.Manager;
using Application.DTO.RunBook;
using Application.Utility.Logging;
using Application.Messages;
using Application.Utility;
using EasyNetQ;
using Application.Utility.Translators;
using Application.DTO.ActionTask;
using Application.Common;
using Application.Manager.ServiceContract;
using Application.DTO.Worksheet;
using Application.Snapshot;

namespace Worker.AutomationHandlers
{
    public class RunbookHandler : IRunbookHandler
    {



        private readonly ILogger _logger;
        private readonly IAutomationBusinessManager _automationManager;
        private readonly IActionTaskManager _actionTaskManager;
        private readonly IEntityTranslatorService _translator;
        private readonly IActionTaskHandler _actionTaskHandler;
        private readonly IConditionHandler _conditionHandler;
        private readonly IWorksheetServiceManager _worksheetManager;
        private readonly IBus _bus;

        public RunbookHandler(IAutomationBusinessManager automationManager, ILogger logger,
            IActionTaskManager actionTaskManager, IBus bus, IEntityTranslatorService translator,
            IWorksheetServiceManager worksheetManager,
            IActionTaskHandler actionTaskHandler, IConditionHandler conditionHandler)
        {
            _automationManager = automationManager;
            _actionTaskManager = actionTaskManager;
            _actionTaskHandler = actionTaskHandler;
            _conditionHandler = conditionHandler;
            _worksheetManager = worksheetManager;
            _translator = translator;
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// Initiate the automation from Gateway and Parent Automation
        /// </summary>
        /// <param name="message"></param>
        public void Execute(AutomationMessage message)
        {
            try
            {
                if (message != null)
                {
                    AutomationDTO entity = _automationManager.GetbyId(message.AutomationId);
                    RunbookEntity runbook = entity.runbookContent;
                    // Id for the start component 
                    string sourceId = runbook.start.Id;
                    string incidentId = message.IncidentId;
                    string automationId = message.AutomationId;
                    AutomationMessage parentAutomations = message.Parent;
                    string processId = Guid.NewGuid().ToString();
                    string requestedBy = message.RequestedBy;
                    DateTime requestedOn = message.RequestedOn;
                    // Parameters for particular automation
                    AutomationParameter parameters = message.Parameters;
                    //Create Worksheet
                    SheetSnapshot sheet = new SheetSnapshot()
                    {
                        AutomationId = automationId,
                        AlertId = incidentId,
                        AssignedTo = requestedBy,
                        Summary = entity.name + " process started",
                        Description = "",
                        IsActive = true,
                        ProcessId = processId,
                        CreatedBy = requestedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    string sheetId = _worksheetManager.AddSheet(sheet);
                    this.Execute(runbook, parameters, incidentId, processId, sheetId, automationId,
                       sourceId, requestedBy, parentAutomations);


                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable execute the runbook", ex, message);
            }
        }


        /// <summary>
        /// Action task executed by worker followed by remote, sending the response for start the next task
        /// This function will initiate the next task in the automation
        /// </summary>
        /// <param name="message"></param>
        public void Execute(ActionTaskResponseMessage message)
        {
            try
            {
                if (message != null)
                {
                    string sourceId = message.ActionIdInRunBook;
                    string incidentId = message.IncidentId;
                    string automationId = message.AutomationId;
                    string processId = message.ProcessId;
                    string requestedBy = message.RequestedBy;
                    AutomationMessage parentAutomations = message.Parent;
                    string sheetId = "";
                    AutomationParameter parameters = message.Parameters;
                    AutomationDTO entity = _automationManager.GetbyId(automationId);
                    RunbookEntity runbook = entity.runbookContent;
                    string condition = "None";
                    if (message.Parameters.Result != null)
                        condition = message.Parameters.Result["Condition"];
                    this.Execute(runbook, parameters, incidentId, processId, sheetId, automationId,
                            sourceId, requestedBy, parentAutomations, condition);
                }
                else
                {
                    _logger.Error("ActionTask response message is null");
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Core of the automation, executes based on flowchart.
        /// </summary>
        /// <param name="runbook"></param>
        /// <param name="parameters"></param>
        /// <param name="sourceId"></param>
        /// <param name="IncidentId"></param>
        /// <param name="automationId"></param>
        /// <param name="sheetId"></param>
        /// <param name="ProcessId"></param>
        /// <param name="RequestedBy"></param>
        /// <param name="ParentAutomations"></param>
        /// <param name="resultcondition"></param>
        /// <param name="IsRemoteDone"></param>

        private void Execute(RunbookEntity Runbook, AutomationParameter Parameters,
            string IncidentId, string ProcessId, string SheetId, string AutomationId,
             string SourceId, string RequestedBy, AutomationMessage ParentAutomations,
            string Resultcondition = "None")
        {
            IEnumerable<Connector> targets = Runbook.connectors.Where(s =>
                            s.cell.Source == SourceId && (s.Condition == Resultcondition
                            || s.Condition == "None" || s.Condition == null));
            if (targets != null)
            {
                foreach (Connector connector in targets)
                {
                    string targetId = connector.cell.Target;
                    Application.DTO.RunBook.Task task = Runbook.tasks.Where(x => x.Id == targetId).FirstOrDefault();
                    if (task != null)
                    {
                        ActionTaskMessage actionTask = _actionTaskManager.GetActionMessagebyId(task.ActionTaskId);
                        if (actionTask != null)
                        {
                            var CallerMessage = new ActionTaskCallerMessage()
                            {
                                ActionTaskId = actionTask.Id,
                                ActionIdInRunBook = task.Id,
                                AutomationId = AutomationId,
                                Code = actionTask.AccessCode,
                                IncidentId = IncidentId,
                                Parameters = Parameters,
                                ConfiguredOutputParams = task.Params.Outputs,
                                Parent = ParentAutomations,
                                ProcessId = ProcessId,
                                RequestedBy = RequestedBy,
                                RequestedOn = DateTime.UtcNow,
                                SheetId = SheetId,
                                Timeout = actionTask.TimeOut
                            };
                            string Queue = "worker1";
                            if (actionTask.Actiontype != "Remote")
                            {
                                Queue = actionTask.Queue;
                                CallerMessage.Code = actionTask.RemoteCode;
                            }
                            else
                            {
                                CallerMessage.Inputs = ActionTaskHelper.UpdateInputParams(actionTask.Inputs, task.Params.Inputs, Parameters);
                            }
                            //create action result in the sheet
                            CallerMessage.ActionResultSheetId = WriteActionResult(SheetId, actionTask.Name, task.Id, RequestedBy, Queue);
                            _bus.Send<ActionTaskCallerMessage>(Queue, CallerMessage);
                        }
                        else
                        {
                            _logger.Error("Action-task is not exist", task.ActionTaskId, task.Label);
                        }
                    }
                    Precondition condition = Runbook.preConditions.Where(x => x.Id == targetId).FirstOrDefault();
                    if (condition != null)
                    {
                        WriteActionResult(SheetId, condition.Label, task.Id, RequestedBy, "Worker1");
                        string result = _conditionHandler.Execute(Parameters, condition.Description);
                        if (result == "None")
                        {
                            // initiate abort model
                        }
                        else
                        {
                            this.Execute(Runbook, Parameters, IncidentId, ProcessId, SheetId, AutomationId,
                                     SourceId, RequestedBy, ParentAutomations, result);
                        }
                    }
                    SubRunbook subRunbook = Runbook.subRunbook.Where(x => x.Id == targetId).FirstOrDefault();
                    if (subRunbook != null)
                    {
                        AutomationDTO entity = _automationManager.GetbyId(subRunbook.SubAutomationId);
                        RunbookEntity subRunbookentity = entity.runbookContent;
                        // current automation will be assigned to Parent automation
                        AutomationMessage automation = new AutomationMessage()
                        {
                            AutomationId = AutomationId,
                            ParentAutomationFlowId = targetId,
                            IncidentId = IncidentId,
                            Parameters = null,
                            ProcessId = ProcessId,
                            RequestedBy = RequestedBy,
                            RequestedOn = DateTime.Now,
                            Parent = ParentAutomations
                        };
                        WriteActionResult(SheetId, subRunbook.Label, targetId, RequestedBy, "", entity.name + " started");

                        //update params
                        string processId = new Guid().ToString(); // new runbook initiation 
                        this.Execute(Runbook, Parameters, IncidentId, processId, SheetId, subRunbook.SubAutomationId,
                                "0", RequestedBy, automation);
                  
                    }
                    End end = Runbook.ends.Where(x => x.Id == targetId).FirstOrDefault();
                    if (end != null)
                    {
                        WriteActionResult(SheetId, "End", targetId, RequestedBy, "worker1");
                        //write in sheet
                        if (ParentAutomations != null)
                        {

                            AutomationDTO entity = _automationManager.GetbyId(ParentAutomations.AutomationId);
                            RunbookEntity ParentRunbook = entity.runbookContent;
                            this.Execute(ParentRunbook, Parameters, IncidentId, ParentAutomations.ProcessId, SheetId, ParentAutomations.AutomationId,
                                ParentAutomations.ParentAutomationFlowId, RequestedBy, ParentAutomations.Parent);
                         }
                    }
                }
            }
            else
            {
                _logger.Error("Connection not made properly", Runbook.start);
            }
        }


        private string WriteActionResult(string SheetId, string name, string taskId, string requestedBy, string Queue, string summary = "")
        {
            ActionResultSnapshot actionResult = new ActionResultSnapshot()
            {
                IsActive = true,
                Name = name,
                ActionTaskId = taskId,
                CreatedOn = DateTime.UtcNow,
                ExecutedBy = requestedBy,
                ExecutedQueue = Queue,
                IsCompletion = false,
                SheetId = SheetId,
                Summary = summary
            };
            return _worksheetManager.AddActionResult(actionResult);
        }

    }
}
