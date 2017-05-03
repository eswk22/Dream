using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Automation
{
    public class AutomationMessage
    {
        public string IncidentId { get; set; }
        public string AutomationId { get; set; }
        public string ParentAutomationFlowId { get; set; }
        public string ProcessId { get; set; }
        public AutomationMessage Parent { get; set; }
        public AutomationParameter Parameters { get; set; }
        public string  RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }

    }


}
