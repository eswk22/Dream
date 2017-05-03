using Application.Common;
using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.Messages;
using Application.Utility.Logging;
using Compiler.Core;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorker.ActionTask
{
    public class ActionTaskHandler : IActionTaskHandler
    {
        private readonly ICodeProcessor _processor;
        private readonly ILogger _logger;
        private readonly IBus _bus;


        /// <summary>
        ///  Handler for executing script in the remote worker  
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="logger"></param>
        public ActionTaskHandler(ICodeProcessor processor, ILogger logger, IBus bus)
        {
            _processor = processor;
            _logger = logger;
            _bus = bus;
        }


        public void execute(ActionTaskCallerMessage actiontaskCaller)
        {
            _logger.Info("Called action task", actiontaskCaller);
            RemoteTaskResponseMessage response = null;
            try
            {
                var globals = new Globals()
                {
                    INPUTS = actiontaskCaller.Inputs,
                    OUTPUTS = new DictionaryWithDefault<string, dynamic>(),
                    RESULTS = new DictionaryWithDefault<string, dynamic>()
                };
                //needs to implement timeout 
                _processor.Execute(actiontaskCaller.Code, globals);
                response = new RemoteTaskResponseMessage()
                {
                    ActionIdInRunBook = actiontaskCaller.ActionIdInRunBook,
                    ActionTaskId = actiontaskCaller.ActionId,
                    AutomationId = actiontaskCaller.AutomationId,
                    IncidentId = actiontaskCaller.IncidentId,
                    Parameters = actiontaskCaller.Parameters,
                };
                response.Parameters.Outputs = globals.OUTPUTS;
                response.Parameters.Result = globals.RESULTS;
                _logger.Info("Result of action task", response);
                _bus.Publish<RemoteTaskResponseMessage>(response);
            }
            catch (Exception ex)
            {
                _logger.Error("Error Occurred while executing the script", ex, actiontaskCaller);
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
