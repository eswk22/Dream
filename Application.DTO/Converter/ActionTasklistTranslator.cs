using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;

namespace Application.DTO.Conversion
{
    public class ActionTasklistTranslator : EntityMapperTranslator<ActionTasklistDTO, ActionTaskSnapshot>
    {
        public override ActionTaskSnapshot BusinessToService(IEntityTranslatorService service, ActionTasklistDTO value)
        {
			//No use case for Business to Service
			throw new NotImplementedException();

		}

		public override ActionTasklistDTO ServiceToBusiness(IEntityTranslatorService service, ActionTaskSnapshot value)
        {
            ActionTasklistDTO _ActionTasklist = null;
            if (value != null)
            {
				_ActionTasklist = new ActionTasklistDTO();
				_ActionTasklist.ActionId = value.Id;
				_ActionTasklist.CreatedBy = value.CreatedBy;
				_ActionTasklist.CreatedOn = value.CreatedOn;
				_ActionTasklist.IsActive = value.IsActive;
				_ActionTasklist.Name = value.Name;
				_ActionTasklist.ModifiedBy = value.ModifiedBy;
				_ActionTasklist.ModifiedOn = value.ModifiedOn;
			}
            return _ActionTasklist;
        }

	}
}
