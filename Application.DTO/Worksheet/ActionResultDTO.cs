using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Worksheet
{
    public class ActionResultDTO
    {
        public string Id { get; set; }
        public string ActionTaskId { get; set; }
        public string Name { get; set; }
        public bool IsCompletion { get; set; }
        public string Condition { get; set; }
        public string Severity { get; set; }
        public string Summary { get; set; }
        public int Duration { get; set; }
        public string ExecutedBy { get; set; }
        public string ExecutedQueue { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
