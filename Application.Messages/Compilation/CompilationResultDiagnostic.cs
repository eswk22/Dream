using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility;

namespace Application.Messages
{
    [Serializable]
    public class CompilationResultDiagnostic
    {
        public string Id { get; set; }
        public DiagnosticSeverity Severity { get; set; }
        public string Message { get; set; }
        public CompilationResultDiagnosticLocation Start { get; set; }
        public CompilationResultDiagnosticLocation End { get; set; }
    }
}
