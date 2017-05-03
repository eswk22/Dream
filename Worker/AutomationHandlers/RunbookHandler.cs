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

namespace Worker.AutomationHandlers
{
    public class RunbookHandler : IRunbookHandler
    {
        private readonly ILogger _logger;
        private readonly IAutomationManager _automationManager;
        private readonly IActionTaskManager _actionTaskManager;
        private readonly IEntityTranslatorService _translator;
        private readonly IActionTaskHandler _actionTaskHandler;
        private readonly IConditionHandler _conditionHandler;
        private readonly IBus _bus;

        public RunbookHandler(IAutomationManager automationManager, ILogger logger,
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
        public void Execute(AutomationMessage message)
        {
            try
            {
                if (message != null)
                {
                    AutomationEntity entity = _automationManager.GetbyId(message.AutomationId);
                    RunbookEntity runbook = entity.runbookContent;
                    string sourceId = runbook.start.Id;
                    string IncidentId = message.IncidentId;
                    string automationId = message.AutomationId;
                    AutomationMessage ParentAutomations = message.Parent;
                    string ProcessId = new Guid().ToString();
                    string RequestedBy = message.RequestedBy;
                    DateTime RequestedOn = message.RequestedOn;
                    string sheetId = "";
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
                    AutomationEntity entity = _automationManager.GetbyId(automationId);
                    RunbookEntity runbook = entity.runbookContent;
                    string condition = message.Parameters.Result["Condition"];
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


        private void Execute(RunbookEntity runbook, AutomationParameter parameters,
            string sourceId, string IncidentId, string automationId, string sheetId,
           string ProcessId, string RequestedBy, AutomationMessage ParentAutomations,
           string resultcondition = "None", bool IsRemoteDone = false)
        {
            IEnumerable<Connector> targets = runbook.connectors.Where(s => s.cell.Source == sourceId &&
                            s.Condition == resultcondition || s.Condition == "None");
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
                            this.updateInputParams();
                            var CallerMessage = _translator.Translate<ActionTaskCallerMessage>(actionTask);
                            CallerMessage.IncidentId = IncidentId;
                            CallerMessage.ActionIdInRunBook = task.Id;
                            CallerMessage.AutomationId = automationId;
                            CallerMessage.Parent = ParentAutomations;
                            CallerMessage.SheetId = "";
                            string Queuename = "worker";
                            if (actionTask.Actiontype == "Remote" && !IsRemoteDone)
                            {
                                Queuename = actionTask.Queue;
                            }
                            _bus.Send<ActionTaskCallerMessage>(actionTask.Queue, CallerMessage);
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
                        AutomationEntity entity = _automationManager.GetbyId(subRunbook.SubAutomationId);
                        RunbookEntity subRunbookentity = entity.runbookContent;
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
                        if(ParentAutomations != null)
                        {

                            AutomationEntity entity = _automationManager.GetbyId(ParentAutomations.AutomationId);
                            RunbookEntity ParentRunbook = entity.runbookContent;

                            this.Execute(ParentRunbook, parameters, ParentAutomations.ParentAutomationFlowId, IncidentId, ParentAutomations.AutomationId,
                                sheetId,ParentAutomations.ProcessId, RequestedBy, ParentAutomations.Parent);
                        }
                    }
                }
            }
            else
            {
                _logger.Error("Connection not made properly", runbook.start);
            }
        }

        private object getProperty(string key)
        {
            object value = null;

            return value;
        }

        private void updateInputParams()
        {
            //foreach (var input in task.param.Where(w => w.paramtype == ParamType.Input))
            //{
            //    switch (input.sourcetype)
            //    {
            //        case SourceType.CNS:
            //            break;
            //        case SourceType.Constant:
            //            actionTask.Inputs[input.label] = input.value;
            //            break;
            //        case SourceType.Default:
            //            break;
            //        case SourceType.Flow:
            //            break;
            //        case SourceType.Output:
            //            break;
            //        case SourceType.Param:
            //            actionTask.Inputs[input.label] = input.value;
            //            break;
            //        case SourceType.Property:
            //            actionTask.Inputs[input.label] = getProperty(input.value);
            //            break;
            //        case SourceType.WSData:
            //            break;
            //    }
            //}
        }
    }
}
