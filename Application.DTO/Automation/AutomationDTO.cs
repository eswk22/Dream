using Application.DTO.RunBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Automation
{
    public class AutomationDTO
    {
        public string automationId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public RunbookEntity runbookContent { get; set; }
        public RunbookEntity runbookException { get; set; }
        public int version { get; set; }
        public bool isLatestVersion { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
