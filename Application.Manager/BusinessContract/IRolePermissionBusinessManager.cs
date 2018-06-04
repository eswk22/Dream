using Application.DTO.Common;
using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IRolePermissionBusinessManager
    {
        RolePermissionDTO GetbyId(string Id);
        string Add(RolePermissionDTO parameterdto);
        void Add(string ParentId, IList<RolePermissionDTO> permittedRoles);
        RolePermissionDTO Update(RolePermissionDTO parameterdto);
        void Update(string ParentId, IList<RolePermissionDTO> permittedRoles);
        IEnumerable<RolePermissionDTO> GetRolePermissions();
        IEnumerable<RolePermissionDTO> GetRolePermissions(string ParentId);
        RolePermissionDTO Save(RolePermissionDTO parameterdto);
        RolePermissionDTO Delete(RolePermissionDTO parameterdto);
        RolePermissionDTO Delete(string Id);
        void DeletebyParantId(string ParentId);

        bool CopyRoles(string OldParentId, string NewParentId);
    }
}
