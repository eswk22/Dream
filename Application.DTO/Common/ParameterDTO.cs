using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common
{
    public class ParameterDTO
    {
        public string Id { get; set; }
        public string ParentType { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public string ParameterType { get; set; }
        public Boolean IsActive { get; set; }
    }
}
