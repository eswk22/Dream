using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.Automation;
using Application.Utility.XmlUtils;
using Application.DTO.RunBook;
using Application.DTO.RunBook.Helper;

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
                if (value.runbookContent != null)
                {
                    snapshot.runbookContent = value.runbookContent.XmlSerializeToString();
                }
                else
                {
                    snapshot.runbookContent = "";
                }
                snapshot.CreatedBy = value.CreatedBy;
                snapshot.CreatedOn = value.CreatedOn;
                if (value.runbookException != null)
                {
                    snapshot.runbookException = value.runbookException.XmlSerializeToString();
                }
                else
                {
                    snapshot.runbookException = "";
                }
                snapshot.IsActive = value.IsActive;
                snapshot.summary = value.summary;
                snapshot.title = value.title;
                snapshot.version = value.version;
                snapshot.ModifiedBy = value.ModifiedBy;
                snapshot.ModifiedOn = value.ModifiedOn;
                snapshot.Params = value.Params;
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
                if (value.runbookContent != string.Empty)
                {
                    dto.runbookContent = value.runbookContent.XmlDeserializeFromString<MxGraphModel>().Root;
                }
                else
                {
                    dto.runbookContent = null;
                }
                dto.CreatedBy = value.CreatedBy;
                dto.CreatedOn = value.CreatedOn;
                if (value.runbookException != string.Empty)
                {
                    dto.runbookException = value.runbookException.XmlDeserializeFromString<MxGraphModel>().Root;
                }
                else
                {
                    dto.runbookException = null;
                }
                dto.IsActive = value.IsActive;
                dto.summary = value.summary;
                dto.title = value.title;
                dto.version = value.version;
                dto.ModifiedBy = value.ModifiedBy;
                dto.ModifiedOn = value.ModifiedOn;
                dto.Params = value.Params;
            }
            return dto;
        }

    }
}
