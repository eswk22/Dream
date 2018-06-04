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
    public class ParameterTranslator : EntityMapperTranslator<ParameterDTO, ParameterSnapshot>
    {
        public override ParameterSnapshot BusinessToService(IEntityTranslatorService service, ParameterDTO value)
        {
            ParameterSnapshot snapshot = null;
            if (value != null)
            {
                snapshot = new ParameterSnapshot();
                snapshot.Id = value.Id;
                snapshot.Name = value.Name;
                snapshot.ParameterType = value.ParameterType;
                snapshot.ParentId = value.ParentId;
                snapshot.ParentType = value.ParentType;
                snapshot.Type = value.Type;
                snapshot.DefaultValue = value.DefaultValue;
                snapshot.IsActive = value.IsActive;
            }
            return snapshot;

        }

        public override ParameterDTO ServiceToBusiness(IEntityTranslatorService service, ParameterSnapshot value)
        {
            ParameterDTO dto = null;
            if (value != null)
            {
                dto = new ParameterDTO();
                dto.Id = value.Id;
                dto.Name = value.Name;
                dto.ParameterType = value.ParameterType;
                dto.ParentId = value.ParentId;
                dto.ParentType = value.ParentType;
                dto.Type = value.Type;
                dto.IsActive = value.IsActive;
                dto.DefaultValue = value.DefaultValue;
            }
            return dto;
        }

    }
}
