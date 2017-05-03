using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Compiler.Core {
    [Serializable]
    public class ProcessingResultDiagnostic {
        public ProcessingResultDiagnostic(Diagnostic diagnostic) {
            Id = diagnostic.Id;
            Severity = Convert.ToString(diagnostic.Severity);
            Message = diagnostic.GetMessage();
            
            var lineSpan = diagnostic.Location.GetMappedLineSpan();
            Start = new ProcessingResultDiagnosticLocation(lineSpan.StartLinePosition);
            End = new ProcessingResultDiagnosticLocation(lineSpan.EndLinePosition);
        }

        public string Id { get; private set; }
        public string Severity { get; private set; }
        public string Message { get; private set; }
        public ProcessingResultDiagnosticLocation Start { get; private set; }
        public ProcessingResultDiagnosticLocation End { get; private set; }
    }
}
