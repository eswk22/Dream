using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Automation
{
    public class AutomationParameter
    {
        public Dictionary<string, dynamic> Params { get; set; }
        public Dictionary<string, dynamic> Flow { get; set; }
        public Dictionary<string, dynamic> CNS { get; set; }
        public Dictionary<string, dynamic> Outputs { get; set; }
        public Dictionary<string, dynamic> Result { get; set; }


    }
}
