using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common
{
    public class RolePermissionDTO
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public Boolean IsView { get; set; }
        public Boolean IsEdit { get; set; }
        public Boolean IsExecute { get; set; }
        public Boolean IsAdmin { get; set; }
        public Boolean IsActive { get; set; }

    }
}
