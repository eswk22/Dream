using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.Common;
using Application.DTO.ActionTask;

namespace Application.DTO.Conversion
{
    public class ActionTaskMessageTranslator : EntityMapperTranslator<ActionTaskDTO, ActionTaskMessage>
    {
        public override ActionTaskMessage BusinessToService(IEntityTranslatorService service, ActionTaskDTO value)
        {
            ActionTaskMessage snapshot = null;
            if (value != null)
            {
                snapshot = new ActionTaskMessage();
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
                snapshot.Inputs = new DictionaryWithDefault<string, dynamic>();
                foreach(var param in value.Parameters.Where(s => s.Type.ToLowerInvariant() == "input"))
                {
                    snapshot.Inputs.Add(param.Name, param.DefaultValue);
                }
                snapshot.Outputs = new DictionaryWithDefault<string, dynamic>();
                foreach (var param in value.Parameters.Where(s => s.Type.ToLowerInvariant() == "output"))
                {
                    snapshot.Outputs.Add(param.Name, param.DefaultValue);
                }
            }
            return snapshot;

        }

        public override ActionTaskDTO ServiceToBusiness(IEntityTranslatorService service, ActionTaskMessage value)
        {
            throw new NotImplementedException();
        //    ActionTaskDTO dto = null;
        //    if (value != null)
        //    {
        //        dto.AccessCode = value.AccessCode;
        //        dto.Actiontype = value.Actiontype;
        //        dto.CreatedBy = value.CreatedBy;
        //        dto.CreatedOn = value.CreatedOn;
        //        dto.Description = value.Description;
        //        dto.ActionId = value.Id;
        //        dto.IsActive = value.IsActive;
        //        dto.IsLatestVersion = value.IsLatestVersion;
        //        dto.LocalCodelanguage = value.LocalCodelanguage;
        //        dto.menupath = value.menupath;
        //        dto.ModifiedBy = value.ModifiedBy;
        //        dto.ModifiedOn = value.ModifiedOn;
        //        dto.module = value.module;
        //        dto.Name = value.Name;
        //        dto.Queue = value.Queue;
        //        dto.RemoteCode = value.RemoteCode;
        //        dto.RemoteCodelanguage = value.RemoteCodelanguage;
        //        dto.Summary = value.Summary;
        //        dto.TimeOut = value.TimeOut;
        //        dto.Version = value.Version;
        //    }
        //    return dto;
        }

    }
}
