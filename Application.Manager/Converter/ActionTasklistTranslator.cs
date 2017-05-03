using Application.Messages;
using Application.Utility.Translators;
using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;

namespace Application.Manager.Conversion
{
    public class ActionTasklistTranslator : EntityMapperTranslator<ActionTasklist, ActionTaskSnapshot>
    {
        public override ActionTaskSnapshot BusinessToService(IEntityTranslatorService service, ActionTasklist value)
        {
			//No use case for Business to Service
			throw new NotImplementedException();

		}

		public override ActionTasklist ServiceToBusiness(IEntityTranslatorService service, ActionTaskSnapshot value)
        {
            ActionTasklist _ActionTasklist = null;
            if (value != null)
            {
				_ActionTasklist = new ActionTasklist();
				_ActionTasklist.ActionId = value.Id;
				_ActionTasklist.CreatedBy = value.CreatedBy;
				_ActionTasklist.CreatedOn = value.CreatedOn;
				_ActionTasklist.IsActive = value.IsActive;
				_ActionTasklist.Name = value.Name;
				_ActionTasklist.UpdatedBy = value.UpdatedBy;
				_ActionTasklist.UpdatedOn = value.UpdatedOn;
			}
            return _ActionTasklist;
        }

	}
}
