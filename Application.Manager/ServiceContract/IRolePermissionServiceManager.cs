using Application.DTO.Common;
using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IRolePermissionServiceManager
    {
        RolePermissionSnapshot GetSnapshotbyId(string Id);
        string Add(RolePermissionSnapshot RolePermissionmessage);
        RolePermissionSnapshot Update(RolePermissionSnapshot snapshot);  
        IEnumerable<RolePermissionSnapshot> GetSnapshots();
        IEnumerable<RolePermissionSnapshot> GetSnapshots(string ParentId);
        RolePermissionSnapshot Save(RolePermissionSnapshot snapshot);
        RolePermissionSnapshot Delete(RolePermissionSnapshot snapshot);
    }
}
