using Application.Common;
using Application.DTO.Automation;
using Application.Utility.Logging;
using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.AutomationHandlers
{
    public class ConditionHandler : IConditionHandler
    {
        private readonly ICodeProcessor _processor;
        private readonly ILogger _logger;

        public ConditionHandler(ICodeProcessor processor, ILogger logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public string Execute(AutomationParameter parameters,string code)
        {
            string result = "None";
            try
            {
                var globals = new Globals()
                {
                    INPUTS = new DictionaryWithDefault<string, dynamic>(),
                    OUTPUTS = new DictionaryWithDefault<string, dynamic>(),
                    RESULTS = new DictionaryWithDefault<string, dynamic>()
                };
                //needs to implement timeout 
                _processor.Execute(code, globals);
                result = "Good";
            }
            catch(Exception ex)
            {
                _logger.Error("Unable to execute the condition", ex, parameters, code);
                result = "None";
            }
            return result;
        }
    }
}
