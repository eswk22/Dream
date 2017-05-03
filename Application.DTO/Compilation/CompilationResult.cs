using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility;

namespace Application.Messages
{
    [Serializable]
    public class CompilationResult
    {
        public string Decompiled { get; set; }
        public bool IsSuccess { get; set; }
       
        public IEnumerable<CompilationResultDiagnostic> Errors { get; set; }

        public IEnumerable<CompilationResultDiagnostic> Warnings { get; set; }
        public IEnumerable<CompilationResultDiagnostic> Infos { get; set; }

    }
}
