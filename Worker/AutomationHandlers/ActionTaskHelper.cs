using Application.Common;
using Application.DTO.Automation;
using Application.DTO.RunBook;
using Application.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.AutomationHandlers
{
    internal static class ActionTaskHelper
    {
        /// <summary>
        /// Update the input parameters with running parameters
        /// </summary>
        /// <param name="PredefinedInputs"  Inputs defined in the actual action task></param>
        /// <param name="ATConfigParam" Inputs configured in the action task in the automation></param>
        /// <param name="parameters" Running auttomation params which include flows,params etc ></param>
        public static Dictionary<string, dynamic> UpdateInputParams(DictionaryWithDefault<string, dynamic> PredefinedInputs, string ATConfigParam, AutomationParameter parameters)
        {
            List<ExtractedParam> ParamsinAT = TransferString2Params(ATConfigParam);
            Dictionary<string, dynamic> Inputs = new Dictionary<string, dynamic>();
            if (ParamsinAT != null)
            {
                foreach (var input in ParamsinAT.Where(w => w.paramtype == ParamType.Input))
                {
                    switch (input.sourcetype)
                    {
                        case SourceType.CNS:
                            Inputs[input.label] = input.value;
                            break;
                        case SourceType.Constant:
                            Inputs[input.label] = input.value;
                            break;
                        case SourceType.Default:
                            Inputs[input.label] = PredefinedInputs[input.label];
                            break;
                        case SourceType.Flow:
                            Inputs[input.label] = parameters.Flow[input.label];
                            break;
                        case SourceType.Param:
                            Inputs[input.label] = parameters.Params[input.label];
                            break;
                        case SourceType.Property:
                            Inputs[input.label] = getProperty(input.value);
                            break;
                        case SourceType.WSData:
                            Inputs[input.label] = getWSData(input.label);
                            break;
                    }
                }
            }
            return Inputs;
        }

        public static AutomationParameter UpdateOutputParams(DictionaryWithDefault<string, dynamic> Outputs, string ATConfigParam, AutomationParameter parameters)
        {
            List<ExtractedParam> ParamsinAT = TransferString2Params(ATConfigParam);
           if (ParamsinAT != null)
            {
                foreach (var output in ParamsinAT.Where(w => w.paramtype == ParamType.Output))
                {
                    switch (output.sourcetype)
                    {
                        case SourceType.CNS:
                            parameters.CNS[output.label] = Outputs[output.label];
                            break;
                        case SourceType.Flow:
                            parameters.Flow[output.label] = Outputs[output.label];
                            break;
                        case SourceType.Param:
                            parameters.Params[output.label] = Outputs[output.label];
                            break;
                         case SourceType.WSData:
                            setWSData(output.label,Outputs[output.label]);
                            break;
                    }
                }
            }
            return parameters;
        }

        private static void setWSData(string label, dynamic dynamic)
        {
            
        }

        public static dynamic getWSData(string label)
        {
            throw new NotImplementedException();
        }

       public static List<ExtractedParam> TransferString2Params(string Param)
        {
            return null;
        }

        public static object getProperty(string key)
        {
            object value = null;

            return value;
        }
    }
}
