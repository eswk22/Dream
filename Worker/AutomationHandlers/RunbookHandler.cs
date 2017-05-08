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
        private readonly IBus _bus;

        public RunbookHandler(IAutomationBusinessManager automationManager, ILogger logger,
            IActionTaskManager actionTaskManager, IBus bus, IEntityTranslatorService translator,
            IActionTaskHandler actionTaskHandler, IConditionHandler conditionHandler)
        {
            _automationManager = automationManager;
            _actionTaskManager = actionTaskManager;
            _actionTaskHandler = actionTaskHandler;
            _conditionHandler = conditionHandler;
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
                    string IncidentId = message.IncidentId;
                    string automationId = message.AutomationId;
                    AutomationMessage ParentAutomations = message.Parent;
                    string ProcessId = Guid.NewGuid().ToString();
                    string RequestedBy = message.RequestedBy;
                    DateTime RequestedOn = message.RequestedOn;
                    string sheetId = "";
                    // Parameters for particular automation
                    AutomationParameter parameters = message.Parameters;
                    Execute(runbook, parameters, sourceId, IncidentId, automationId, sheetId,
                        ProcessId, RequestedBy, ParentAutomations);
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
                    string IncidentId = message.IncidentId;
                    string automationId = message.AutomationId;
                    string ProcessId = message.ProcessId;
                    string RequestedBy = message.RequestedBy;
                    AutomationMessage ParentAutomations = message.Parent;
                    string sheetId = "";
                    AutomationParameter parameters = message.Parameters;
                    AutomationDTO entity = _automationManager.GetbyId(automationId);
                    RunbookEntity runbook = entity.runbookContent;
                    string condition = "None";
                    if(message.Parameters.Result != null)
                        condition = message.Parameters.Result["Condition"];
                    Execute(runbook, parameters, sourceId, IncidentId, automationId, sheetId,
                        ProcessId, RequestedBy, ParentAutomations, condition, true);
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
        private void Execute(RunbookEntity runbook, AutomationParameter parameters,
            string sourceId, string IncidentId, string automationId, string sheetId,
           string ProcessId, string RequestedBy, AutomationMessage ParentAutomations,
           string resultcondition = "None", bool IsRemoteDone = false)
        {
            IEnumerable<Connector> targets = runbook.connectors.Where(s =>
                            s.cell.Source == sourceId && (s.Condition == resultcondition
                            || s.Condition == "None" || s.Condition == null));
            if (targets != null)
            {
                foreach (Connector connector in targets)
                {
                    string targetId = connector.cell.Target;
                    Application.DTO.RunBook.Task task = runbook.tasks.Where(x => x.Id == targetId).FirstOrDefault();
                    if (task != null)
                    {
                        ActionTaskMessage actionTask = _actionTaskManager.GetbyId(task.ActionTaskId);
                        if (actionTask != null)
                        {
                            var CallerMessage = new ActionTaskCallerMessage()
                            {
                                ActionTaskId = actionTask.ActionId,
                                ActionIdInRunBook = task.Id,
                                AutomationId = automationId,
                                Code = actionTask.AccessCode,
                                IncidentId = IncidentId,
                                Parameters = parameters,
                                ConfiguredOutputParams = task.Params.Outputs,
                                Parent = ParentAutomations,
                                ProcessId = ProcessId,
                                RequestedBy = RequestedBy,
                                RequestedOn = DateTime.UtcNow,
                                SheetId = ""
                            };
                            string Queue = "worker1";
                            if (actionTask.Actiontype != "Remote" && !IsRemoteDone)
                            {
                                Queue = actionTask.Queue;
                                CallerMessage.Code = actionTask.RemoteCode;
                            }
                            else
                            {
                                CallerMessage.Inputs =ActionTaskHelper.UpdateInputParams(actionTask.Inputs, task.Params.Inputs, parameters);
                            }
                            _bus.Send<ActionTaskCallerMessage>(Queue, CallerMessage);
                        }
                        else
                        {
                            _logger.Error("Action-task is not exist", task.ActionTaskId, task.Label);
                        }
                    }
                    Precondition condition = runbook.preConditions.Where(x => x.Id == targetId).FirstOrDefault();
                    if (condition != null)
                    {
                        string result = _conditionHandler.Execute(parameters, condition.Description);
                        if (result == "None")
                        {
                            // initiate abort model
                        }
                        else
                        {

                            this.Execute(runbook, parameters, sourceId, IncidentId, automationId, sheetId,
                                         ProcessId, RequestedBy, ParentAutomations, result);
                        }
                    }
                    SubRunbook subRunbook = runbook.subRunbook.Where(x => x.Id == targetId).FirstOrDefault();
                    if (subRunbook != null)
                    {
                        AutomationDTO entity = _automationManager.GetbyId(subRunbook.SubAutomationId);
                        RunbookEntity subRunbookentity = entity.runbookContent;
                        // current automation will be assigned to Parent automation
                        AutomationMessage automation = new AutomationMessage()
                        {
                            AutomationId = automationId,
                            ParentAutomationFlowId = targetId,
                            IncidentId = IncidentId,
                            Parameters = null,
                            ProcessId = ProcessId,
                            RequestedBy = RequestedBy,
                            RequestedOn = DateTime.Now,
                            Parent = ParentAutomations
                        };

                        //update params
                        this.Execute(runbook, parameters, "0", IncidentId, subRunbook.SubAutomationId, sheetId,
                                new Guid().ToString(), RequestedBy, automation);

                    }
                    End end = runbook.ends.Where(x => x.Id == targetId).FirstOrDefault();
                    if (end != null)
                    {
                        //write in sheet
                        if (ParentAutomations != null)
                        {

                            AutomationDTO entity = _automationManager.GetbyId(ParentAutomations.AutomationId);
                            RunbookEntity ParentRunbook = entity.runbookContent;

                            this.Execute(ParentRunbook, parameters, ParentAutomations.ParentAutomationFlowId, IncidentId, ParentAutomations.AutomationId,
                                sheetId, ParentAutomations.ProcessId, RequestedBy, ParentAutomations.Parent);
                        }
                    }
                }
            }
            else
            {
                _logger.Error("Connection not made properly", runbook.start);
            }
        }
        

       
    }
}
