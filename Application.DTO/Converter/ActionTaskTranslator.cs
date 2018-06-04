using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;

namespace Application.DTO.Conversion
{
    public class ActionTaskTranslator : EntityMapperTranslator<ActionTaskDTO, ActionTaskSnapshot>
    {
        public override ActionTaskSnapshot BusinessToService(IEntityTranslatorService service, ActionTaskDTO value)
        {
            ActionTaskSnapshot snapshot = null;
            if (value != null)
            {
                snapshot = new ActionTaskSnapshot();
                snapshot.AccessCode = value.LocalCode;
                snapshot.Actiontype = value.Type;
                snapshot.CreatedBy = value.CreatedBy;
                snapshot.CreatedOn = value.CreatedOn;
                snapshot.Description = value.Description;
                snapshot.Id = value.ActionTaskId;
                snapshot.IsActive = value.IsActive;
                snapshot.LocalCodelanguage = value.LocalLanguage;
                snapshot.menupath = value.FolderPath;
                snapshot.ModifiedBy = value.ModifiedBy;
                snapshot.ModifiedOn = value.ModifiedOn;
                snapshot.module = value.Namespace;
                snapshot.Name = value.Name;
                snapshot.Queue = value.Queuename;
                snapshot.RemoteCode = value.RemoteCode;
                snapshot.RemoteCodelanguage = value.RemoteLanguage;
                snapshot.Summary = value.Summary;
                snapshot.TimeOut = value.Timeout;
                snapshot.Version = value.Version;
                snapshot.Status = value.Status;
            }
            return snapshot;

        }

        public override ActionTaskDTO ServiceToBusiness(IEntityTranslatorService service, ActionTaskSnapshot value)
        {
            ActionTaskDTO dto = null;
            if (value != null)
            {
                dto = new ActionTaskDTO();
                dto.LocalCode = value.AccessCode;
                dto.Type = value.Actiontype;
                dto.CreatedBy = value.CreatedBy;
                dto.CreatedOn = value.CreatedOn;
                dto.Description = value.Description;
                dto.ActionTaskId = value.Id;
                dto.IsActive = value.IsActive;
                dto.LocalLanguage = value.LocalCodelanguage;
                dto.FolderPath = value.menupath;
                dto.ModifiedBy = value.ModifiedBy;
                dto.ModifiedOn = value.ModifiedOn;
                dto.Namespace = value.module;
                dto.Name = value.Name;
                dto.Queuename = value.Queue;
                dto.RemoteCode = value.RemoteCode;
                dto.RemoteLanguage = value.RemoteCodelanguage;
                dto.Summary = value.Summary;
                dto.Timeout = value.TimeOut;
                dto.Version = value.Version;
                dto.Status = value.Status;
            }
            return dto;
        }

    }
}
