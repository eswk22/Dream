using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.Automation;

namespace Application.DTO.Conversion
{
    public class AutomationTranslator : EntityMapperTranslator<AutomationDTO, AutomationSnapshot>
    {
        public override AutomationSnapshot BusinessToService(IEntityTranslatorService service, AutomationDTO value)
        {
			AutomationSnapshot snapshot = null;
			if (value != null)
			{
				snapshot = new AutomationSnapshot();
				snapshot.isLatestVersion = value.isLatestVersion;
				snapshot.Id = value.automationId;
				snapshot.name = value.name;
				snapshot.runbookContent = value.runbookContent;
				snapshot.CreatedBy = value.CreatedBy;
				snapshot.CreatedOn = value.CreatedOn;
				snapshot.runbookException = value.runbookException;
				snapshot.IsActive = value.IsActive;
				snapshot.summary = value.summary;
				snapshot.title = value.title;
				snapshot.version = value.version;
				snapshot.ModifiedBy = value.ModifiedBy;
				snapshot.ModifiedOn = value.ModifiedOn;
			}
			return snapshot;

		}

		public override AutomationDTO ServiceToBusiness(IEntityTranslatorService service, AutomationSnapshot value)
        {
            AutomationDTO dto = null;
            if (value != null)
            {
                dto = new AutomationDTO();
                dto.isLatestVersion = value.isLatestVersion;
                dto.automationId = value.Id;
                dto.name = value.name;
                dto.runbookContent = value.runbookContent;
                dto.CreatedBy = value.CreatedBy;
                dto.CreatedOn = value.CreatedOn;
                dto.runbookException = value.runbookException;
                dto.IsActive = value.IsActive;
                dto.summary = value.summary;
                dto.title = value.title;
                dto.version = value.version;
                dto.ModifiedBy = value.ModifiedBy;
                dto.ModifiedOn = value.ModifiedOn;
            }
            return dto;
        }

	}
}
