using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Gateway
{
    public class GatewayCallerMessage
    {
        public string MessageId { get; set; }
        public string Name { get; set; }
        public Int64 Interval { get; set; }
        public Int32 Order { get; set; }
        public string AutomationId { get; set; }
        public bool IsActive { get; set; }
        public string Query { get; set; }
        public string Script { get; set; }
        public string EventId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string GatewayName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime LastRunTime { get; set; }
    }
}
