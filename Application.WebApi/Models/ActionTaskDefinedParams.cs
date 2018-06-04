using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApi.Models
{
    public class ActionTaskDefinedParams
    {
        public DictionaryWithDefault<string,dynamic> Inputs { get; set; }
        public DictionaryWithDefault<string, dynamic> Outputs { get; set; }
    }
}