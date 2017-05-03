using Application.Common;
using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.DTO.RunBook;
using Application.Manager;
using Application.Messages;
using Application.Utility.Logging;
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
        private readonly IAutomationManager _automationManager;


        /// <summary>
        ///  Handler for executing script in the remote worker  
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="logger"></param>
        public ActionTaskHandler(ICodeProcessor processor, ILogger logger, IBus bus,
            IActionTaskManager _actionTaskManager, IAutomationManager automationManager)
        {
            _processor = processor;
            _logger = logger;
            _bus = bus;
            _automationManager = automationManager;
        }


        public void Execute(ActionTaskCallerMessage message)
        {
            _logger.Info("Called action task", message);
            ActionTaskResponseMessage response = null;
            try
            {
                AutomationParameter result = this.Execute(message.Inputs, message.Parameters, message.Code);
                //update params
                response = new ActionTaskResponseMessage()
                {
                    ActionIdInRunBook = message.ActionIdInRunBook,
                    ActionTaskId = message.ActionId,
                    AutomationId = message.AutomationId,
                    IncidentId = message.IncidentId,
                    Parent = message.Parent,
                    ProcessId = message.ProcessId,
                    RequestedBy = message.RequestedBy,
                    RequestedOn = message.RequestedOn,
                    Parameters = result
                };
                _logger.Info("Result of action task", response);
                _bus.Publish<ActionTaskResponseMessage>(response);
            }
            catch (Exception ex)
            {
                _logger.Error("Error Occurred while executing the script", ex, message);
            }
        }

        private AutomationParameter updateOutputParams(AutomationParameter paramSet, Globals globals)
        {
            throw new NotImplementedException();
        }

        public void Execute(RemoteTaskResponseMessage message)
        {
            ActionTaskResponseMessage response = null;
            try
            {
                if (message != null)
                {
                    string actionId = message.ActionTaskId;
                    string sheetId = "";
                    //write in the sheet
                    ActionTaskMessage actionTask = _actionTaskManager.GetbyId(actionId);
                    var result = this.Execute(null, message.Parameters, actionTask.AccessCode);
                    response = new ActionTaskResponseMessage()
                    {
                        ActionIdInRunBook = message.ActionIdInRunBook,
                        ActionTaskId = message.ActionTaskId,
                        AutomationId = message.AutomationId,
                        IncidentId = message.IncidentId,
                        Parent = message.Parent,
                        ProcessId = message.ProcessId,
                        RequestedBy = message.RequestedBy,
                        RequestedOn = message.RequestedOn,
                        Parameters = result
                    };
                    _bus.Publish<ActionTaskResponseMessage>(response);
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



        private AutomationParameter Execute(DictionaryWithDefault<string, dynamic> Inputs,
            AutomationParameter automationParam, string AccessCode)
        {
            var globals = new Globals()
            {
                INPUTS = Inputs,
                OUTPUTS = new DictionaryWithDefault<string, dynamic>(),
                RESULTS = new DictionaryWithDefault<string, dynamic>()
            };
            //needs to implement timeout 
            _processor.Execute(AccessCode, globals);
            //write in sheet
            return this.updateOutputParams(automationParam, globals);

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
