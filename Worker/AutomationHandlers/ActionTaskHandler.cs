using Application.Common;
using Application.DTO;
using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.DTO.RunBook;
using Application.DTO.Worksheet;
using Application.Manager;
using Application.Manager.ServiceContract;
using Application.Messages;
using Application.Snapshot;
using Application.Utility.Logging;
using Application.Utility.Translators;
using Compiler.Core;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.AutomationHandlers
{
    public class ActionTaskHandler : IActionTaskHandler
    {
        private readonly ICodeProcessor _processor;
        private readonly ILogger _logger;
        private readonly IBus _bus;
        private readonly IActionTaskManager _actionTaskManager;
        private readonly IAutomationBusinessManager _automationManager;
        private readonly IEntityTranslatorService _translator;
        private readonly IWorksheetServiceManager _worksheetManager;


        /// <summary>
        ///  Handler for executing script in the remote worker  
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="logger"></param>
        public ActionTaskHandler(ICodeProcessor processor, ILogger logger, IBus bus,
            IActionTaskManager _actionTaskManager, IAutomationBusinessManager automationManager,
            IEntityTranslatorService translator, IWorksheetServiceManager worksheetManager)
        {
            _processor = processor;
            _logger = logger;
            _bus = bus;
            _automationManager = automationManager;
            _translator = translator;
        }


        public void Execute(ActionTaskCallerMessage message)
        {
            _logger.Info("Called action task", message);
            ActionTaskResponseMessage response = null;
            try
            {
                var globals = new Globals()
                {
                    INPUTS = message.Inputs,
                    OUTPUTS = new Dictionary<string, dynamic>(),
                    RESULTS = new Dictionary<string, dynamic>()
                };

                //needs to implement timeout 
                _processor.Execute(message.Code, globals, message.Timeout);

                message.Parameters.Result = globals.RESULTS;
                //update params

                AutomationParameter parameters = ActionTaskHelper.UpdateOutputParams(globals.OUTPUTS, message.ConfiguredOutputParams, message.Parameters);
                response = _translator.Translate<ActionTaskResponseMessage>(message);
                response.Parameters = parameters;
                response.Inputs = null;
                _logger.Info("Result of action task", response);
                UpdateActionResult(message.SheetId, message.ActionIdInRunBook, globals.RESULTS);
                _bus.Send<ActionTaskResponseMessage>("worker", response);
            }
            catch(TimeoutException ex)
            {
                _logger.Error("Action Task timed out", ex, message);
                //initiate the abort model
            }
            catch (Exception ex)
            {
                _logger.Error("Error Occurred while executing the script", ex, message);
            }
        }


        public void Execute(RemoteTaskResponseMessage message)
        {
            try
            {
                if (message != null)
                {
                    string actionId = message.ActionTaskId;
                    //write in the sheet
                    ActionTaskDTO actionTask = _actionTaskManager.GetbyId(actionId);
                    ActionTaskCallerMessage CallerMessage = _translator.Translate<ActionTaskCallerMessage>(message);
                    CallerMessage.Code = actionTask.LocalCode;
                    this.Execute(CallerMessage);
                }
                else
                {
                    _logger.Error("ActionTask response message is null");
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error : ", ex, message);
            }
        }

        private void UpdateActionResult(string SheetId, string ActionResultId, Dictionary<string, dynamic> Result)
        {
            ActionResultSnapshot actionResult = _worksheetManager.GetActionResultSnapshotbyId(ActionResultId);
            actionResult.Condition = Result["condition"];
            actionResult.Detail = Result["detail"];
            actionResult.Duration = Convert.ToInt16(DateTime.UtcNow.Subtract(actionResult.CreatedOn).TotalMinutes);
            actionResult.IsCompletion = true;
            actionResult.ModifiedOn = DateTime.UtcNow;
            actionResult.Severity = Result["severity"];
            actionResult.Summary = Result["summary"];
            _worksheetManager.UpdateActionResult(actionResult);

        }




    }

    /// <summary>
    /// Global parameter for script
    /// </summary>
    public class Globals
    {
        public Dictionary<string, dynamic> INPUTS { get; set; }
        public Dictionary<string, dynamic> OUTPUTS { get; set; }
        public Dictionary<string, dynamic> RESULTS { get; set; }


    }
}
