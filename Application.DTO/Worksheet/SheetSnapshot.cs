using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Worksheet
{
    public class SheetSnapshot
    {
        public string Id { get; set; }
        public string AutomationId { get; set; }
        public string number { get; set; }
        public string AlertId { get; set; }
        public string ProcessId { get; set; }
        public string AssignedTo { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string WorkNotes { get; set; }
        public string Condition { get; set; }
        public string Severity { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public Dictionary<string, dynamic> WSData { get; set; }

        internal static CreateIndexDescriptor IndexDescriptor
        {
            get
            {
                return new CreateIndexDescriptor("sheet")
                .Mappings(ms => ms
                     .Map<SheetSnapshot>(m => m.Properties(ps => ps
                                     .Text(s => s.Name(c => c.AutomationId))
                                     .Text(s => s.Name(c => c.number))
                                     .Text(s => s.Name(c => c.AlertId))
                                     .Text(s => s.Name(c => c.ProcessId))
                                     .Text(s => s.Name(c => c.AssignedTo))
                                     .Text(s => s.Name(c => c.Summary))
                                     .Text(s => s.Name(c => c.Description))
                                     .Text(s => s.Name(c => c.WorkNotes).Index(false))
                                     .Text(s => s.Name(c => c.Condition))
                                     .Text(s => s.Name(c => c.Severity))
                                     .Text(s => s.Name(c => c.CreatedOn))
                                     .Date(s => s.Name(c => c.CreatedBy))
                                     .Text(s => s.Name(c => c.ModifiedBy))
                                     .Date(s => s.Name(c => c.ModifiedOn))
                                     .Boolean(s => s.Name(c => c.IsActive))
                                     .Object<Dictionary<string, dynamic>>(s => s.Name(c => c.WSData))
                                )));
            }
        }

    }



}
