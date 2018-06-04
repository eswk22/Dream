using Application.Common;
using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.Messages;
using Application.Utility.Logging;
using Application.Utility.Translators;
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
        private readonly IEntityTranslatorService _translator;


        /// <summary>
        ///  Handler for executing script in the remote worker  
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="logger"></param>
        public ActionTaskHandler(ICodeProcessor processor, ILogger logger, IBus bus,
            IEntityTranslatorService translator)
        {
            _processor = processor;
            _logger = logger;
            _bus = bus;
            _translator = translator;
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
                    OUTPUTS = new Dictionary<string, dynamic>(),
                    RESULTS = new Dictionary<string, dynamic>()
                };
                //needs to implement timeout 
                _processor.Execute(actiontaskCaller.Code, globals,actiontaskCaller.Timeout);
                response = _translator.Translate<RemoteTaskResponseMessage>(actiontaskCaller);
                response.Parameters.Outputs = globals.OUTPUTS;
                response.Inputs = globals.INPUTS;
                response.Parameters.Result = globals.RESULTS;
                _logger.Info("Result of action task", response);
                _bus.Send<RemoteTaskResponseMessage>("worker", response);
            }
            catch (TimeoutException ex)
            {
                _logger.Error("Action Task timed out", ex, actiontaskCaller);
                //initiate the abort model
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
        public Dictionary<string, dynamic> INPUTS { get; set; }
        public Dictionary<string, dynamic> OUTPUTS { get; set; }
        public Dictionary<string, dynamic> RESULTS { get; set; }


    }
}
