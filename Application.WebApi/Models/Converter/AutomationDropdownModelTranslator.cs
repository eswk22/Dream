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

namespace Application.WebApi.Models
{
    public class AutomationDropdownModelTranslator : EntityMapperTranslator<AutomationDropdownModel,AutomationDTO>
    {
        public override AutomationDTO BusinessToService(IEntityTranslatorService service, AutomationDropdownModel value)
        {
            //no use case
            throw new NotImplementedException();
        }

        public override AutomationDropdownModel ServiceToBusiness(IEntityTranslatorService service, AutomationDTO value)
        {
            AutomationDropdownModel model = null;
            if (value != null)
            {
                if (value.IsActive)
                {
                    model = new AutomationDropdownModel();
                    model.Id = value.automationId;
                    model.Name = value.name;
                }
            }
            return model;
        }

    }
}
