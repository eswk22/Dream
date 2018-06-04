using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApi.Models
{
    public class PropertyModel
    {
        public string PropertyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Namespace { get; set; }
        public string ProprtyType { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}