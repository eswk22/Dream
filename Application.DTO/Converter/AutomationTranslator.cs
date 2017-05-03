using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;

namespace Application.DTO.Conversion
{
    public class ActionTaskTranslator : EntityMapperTranslator<ActionTaskMessage, ActionTaskSnapshot>
    {
        public override ActionTaskSnapshot BusinessToService(IEntityTranslatorService service, ActionTaskMessage value)
        {
			ActionTaskSnapshot _ActionTaskSnapshot = null;
			if (value != null)
			{
				_ActionTaskSnapshot = new ActionTaskSnapshot();
				_ActionTaskSnapshot.AccessCode = value.AccessCode;
				_ActionTaskSnapshot.Id = value.ActionId;
				_ActionTaskSnapshot.Actiontype = value.Actiontype;
				_ActionTaskSnapshot.Codelanguage = value.Codelanguage;
				_ActionTaskSnapshot.CreatedBy = value.CreatedBy;
				_ActionTaskSnapshot.CreatedOn = value.CreatedOn;
				_ActionTaskSnapshot.Description = value.Description;
				//	_ActionTaskSnapshot.Inputs = value.Inputs;
				_ActionTaskSnapshot.IsActive = value.IsActive;
				_ActionTaskSnapshot.menupath = value.menupath;
				//	_ActionTaskSnapshot.MockInputs = value.MockInputs;
				_ActionTaskSnapshot.module = value.module;
				_ActionTaskSnapshot.Name = value.Name;
				//	_ActionTaskSnapshot.Outputs = value.Outputs;
				_ActionTaskSnapshot.RemoteCode = value.RemoteCode;
				//	_ActionTaskSnapshot.Results = value.Results;
				_ActionTaskSnapshot.Summary = value.Summary;
				_ActionTaskSnapshot.TimeOut = value.TimeOut;
				_ActionTaskSnapshot.UpdatedBy = value.UpdatedBy;
				_ActionTaskSnapshot.UpdatedOn = value.UpdatedOn;
			}
			return _ActionTaskSnapshot;

		}

		public override ActionTaskMessage ServiceToBusiness(IEntityTranslatorService service, ActionTaskSnapshot value)
        {
            ActionTaskMessage _ActionTaskMessage = null;
            if (value != null)
            {
                _ActionTaskMessage = new ActionTaskMessage();
                _ActionTaskMessage.AccessCode = value.AccessCode;
				_ActionTaskMessage.ActionId = value.Id;
				_ActionTaskMessage.Actiontype = value.Actiontype;
				_ActionTaskMessage.Codelanguage = value.Codelanguage;
				_ActionTaskMessage.CreatedBy = value.CreatedBy;
				_ActionTaskMessage.CreatedOn = value.CreatedOn;
				_ActionTaskMessage.Description = value.Description;
			//	_ActionTaskMessage.Inputs = value.Inputs;
				_ActionTaskMessage.IsActive = value.IsActive;
				_ActionTaskMessage.menupath = value.menupath;
			//	_ActionTaskMessage.MockInputs = value.MockInputs;
				_ActionTaskMessage.module = value.module;
				_ActionTaskMessage.Name = value.Name;
			//	_ActionTaskMessage.Outputs = value.Outputs;
				_ActionTaskMessage.RemoteCode = value.RemoteCode;
			//	_ActionTaskMessage.Results = value.Results;
				_ActionTaskMessage.Summary = value.Summary;
				_ActionTaskMessage.TimeOut = value.TimeOut;
				_ActionTaskMessage.UpdatedBy = value.UpdatedBy;
				_ActionTaskMessage.UpdatedOn = value.UpdatedOn;
			}
            return _ActionTaskMessage;
        }

	}
}
