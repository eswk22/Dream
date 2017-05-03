using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility;

namespace Application.Messages
{
    [Serializable]
    public class CompilationArguments
    {
        public string Code { get; set; }
        public LanguageIdentifier SourceLanguage { get; set; }
        public LanguageIdentifier TargetLanguage { get; set; }
        public bool OptimizationsEnabled { get; set; }
    }
}
