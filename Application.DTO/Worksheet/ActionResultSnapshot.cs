using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Worksheet
{
    public class ActionResultSnapshot
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
        public string SheetId { get; set; }
        internal static CreateIndexDescriptor IndexDescriptor
        {
            get
            {
                return new CreateIndexDescriptor("sheetrow")
                .Mappings(ms => ms
                     .Map<ActionResultSnapshot>(m => m.Properties(ps => ps
                                     .Text(s => s.Name(c => c.ActionTaskId))
                                     .Text(s => s.Name(c => c.Condition))
                                     .Text(s => s.Name(c => c.Detail).Index(false))
                                     .Text(s => s.Name(c => c.Duration))
                                     .Text(s => s.Name(c => c.ExecutedBy))
                                     .Text(s => s.Name(c => c.Summary))
                                     .Text(s => s.Name(c => c.ExecutedQueue))
                                     .Text(s => s.Name(c => c.IsCompletion))
                                     .Text(s => s.Name(c => c.Condition))
                                     .Text(s => s.Name(c => c.Severity))
                                     .Text(s => s.Name(c => c.Name))
                                     .Text(s => s.Name(c => c.SheetId))
                                     .Date(s => s.Name(c => c.CreatedOn))
                                     .Date(s => s.Name(c => c.ModifiedOn))
                                     .Boolean(s => s.Name(c=> c.IsActive))
                                ).Parent<SheetSnapshot>()
                                .DateDetection(true)));

            }
        }


    }
}
