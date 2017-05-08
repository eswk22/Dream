using Application.Common;
using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.DTO.RunBook;
using Application.Manager;
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


        /// <summary>
        ///  Handler for executing script in the remote worker  
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="logger"></param>
        public ActionTaskHandler(ICodeProcessor processor, ILogger logger, IBus bus,
            IActionTaskManager _actionTaskManager, IAutomationBusinessManager automationManager,
            IEntityTranslatorService translator)
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
                    //  INPUTS = message.Inputs,
                    OUTPUTS = new DictionaryWithDefault<string, dynamic>(),
                    RESULTS = new DictionaryWithDefault<string, dynamic>()
                };

                //needs to implement timeout 
                _processor.Execute(message.Code, globals);

                message.Parameters.Result = globals.RESULTS;
                //update params

                AutomationParameter parameters = ActionTaskHelper.UpdateOutputParams(globals.OUTPUTS, message.ConfiguredOutputParams, message.Parameters);
                response = _translator.Translate<ActionTaskResponseMessage>(message);
                response.Parameters = parameters;
                response.Inputs = null;
                _logger.Info("Result of action task", response);
                _bus.Send<ActionTaskResponseMessage>("worker", response);
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
                    ActionTaskMessage actionTask = _actionTaskManager.GetbyId(actionId);
                    ActionTaskCallerMessage CallerMessage = _translator.Translate<ActionTaskCallerMessage>(message);
                    CallerMessage.Code = actionTask.AccessCode;
                    this.Execute(CallerMessage);
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





    }

    /// <summary>
    /// Global parameter for script
    /// </summary>
    public class Globals
    {
        public DictionaryWithDefault<string, dynamic> INPUTS { get; set; }
        public DictionaryWithDefault<string, dynamic> OUTPUTS { get; set; }
        public DictionaryWithDefault<string, dynamic> RESULTS { get; set; }


    }
}
