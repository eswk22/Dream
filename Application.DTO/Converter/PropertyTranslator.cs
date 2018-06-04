using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.Property;

namespace Application.DTO.Conversion
{
    public class PropertyTranslator : EntityMapperTranslator<PropertyDTO, PropertySnapshot>
    {
        public override PropertySnapshot BusinessToService(IEntityTranslatorService service, PropertyDTO value)
        {
			PropertySnapshot snapshot = null;
			if (value != null)
			{
				snapshot = new PropertySnapshot();
                snapshot.CreatedBy = value.CreatedBy;
                snapshot.CreatedOn = value.CreatedOn;
                snapshot.Description = value.Description;
                snapshot.Id = value.PropertyId;
                snapshot.ModifiedBy = value.ModifiedBy;
                snapshot.ModifiedOn = value.ModifiedOn;
                snapshot.Name = value.Name;
                snapshot.Namespace = value.Namespace;
                snapshot.Value = value.Value;
                snapshot.IsActive = value.IsActive;
			}
			return snapshot;

		}

		public override PropertyDTO ServiceToBusiness(IEntityTranslatorService service, PropertySnapshot value)
        {
            PropertyDTO dto = null;
            if (value != null)
            {
                dto = new PropertyDTO();
                dto.CreatedBy = value.CreatedBy;
                dto.CreatedOn = value.CreatedOn;
                dto.Description = value.Description;
                dto.PropertyId = value.Id;
                dto.ModifiedBy = value.ModifiedBy;
                dto.ModifiedOn = value.ModifiedOn;
                dto.Name = value.Name;
                dto.Namespace = value.Namespace;
                dto.Value = value.Value;
                dto.IsActive = value.IsActive;
            }
            return dto;
        }

	}
}
