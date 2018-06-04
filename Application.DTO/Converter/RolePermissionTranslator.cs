using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.Common;

namespace Application.DTO.Conversion
{
    public class RolePermissionTranslator : EntityMapperTranslator<RolePermissionDTO, RolePermissionSnapshot>
    {
        public override RolePermissionSnapshot BusinessToService(IEntityTranslatorService service, RolePermissionDTO value)
        {
            RolePermissionSnapshot snapshot = null;
            if (value != null)
            {
                snapshot = new RolePermissionSnapshot();
                snapshot.Id = value.Id;
                snapshot.IsAdmin = value.IsAdmin;
                snapshot.IsEdit = value.IsEdit;
                snapshot.IsExecute = value.IsExecute;
                snapshot.IsView = value.IsView;
                snapshot.ParentId = value.ParentId;
                snapshot.RoleId = value.RoleId;
                snapshot.RoleName = value.RoleName;
                snapshot.IsActive = value.IsActive;
            }
            return snapshot;

        }

        public override RolePermissionDTO ServiceToBusiness(IEntityTranslatorService service, RolePermissionSnapshot value)
        {
            RolePermissionDTO dto = null;
            if (value != null)
            {
                dto = new RolePermissionDTO();
                dto.Id = value.Id;
                dto.IsAdmin = value.IsAdmin;
                dto.IsEdit = value.IsEdit;
                dto.IsExecute = value.IsExecute;
                dto.IsView = value.IsView;
                dto.ParentId = value.ParentId;
                dto.RoleId = value.RoleId;
                dto.RoleName = value.RoleName;
                dto.IsActive = value.IsActive;
            }
            return dto;
        }

    }
}
