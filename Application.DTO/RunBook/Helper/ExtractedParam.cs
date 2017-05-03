using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility = Application.Utility;

namespace Application.DTO.RunBook
{
    public class ExtractedParam
    {
        public string label { get; set; }
        public Utility.SourceType sourcetype { get; set; }
        public string value { get; set; }
        public Utility.ParamType paramtype { get; set; }
    }
}
