using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility;

namespace Application.Messages
{
    [Serializable]
    public class CompilationResultDiagnosticLocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
