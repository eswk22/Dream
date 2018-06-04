using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Worksheet
{
    public class SheetDTO
    {
        public string Id { get; set; }
        public string AutomationId { get; set; }
        public string number { get; set; }
        public string AlertId { get; set; }
        public string CorrelationId { get; set; }
        public string AssignedTo { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string WorkNotes { get; set; }
        public string Condition { get; set; }
        public string Severity { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public Dictionary<string, dynamic> WSData { get; set; }

 
    }



}
