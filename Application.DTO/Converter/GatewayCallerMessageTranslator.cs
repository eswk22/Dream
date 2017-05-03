using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.Gateway;

namespace Application.DTO.Conversion
{
    public class GatewayCallerMessageTranslator : EntityMapperTranslator<GatewayCallerMessage, GatewaySnapshot>
    {
        public override GatewaySnapshot BusinessToService(IEntityTranslatorService service, GatewayCallerMessage value)
        {
			GatewaySnapshot _GatewaySnapshot = null;
			if (value != null)
			{
				_GatewaySnapshot = new GatewaySnapshot();
				_GatewaySnapshot.EventId = value.EventId;
				_GatewaySnapshot.Id = value.MessageId;
				_GatewaySnapshot.GatewayName = value.GatewayName;
				_GatewaySnapshot.Interval = value.Interval;
				_GatewaySnapshot.CreatedBy = value.CreatedBy;
				_GatewaySnapshot.CreatedOn = value.CreatedOn;
				_GatewaySnapshot.LastRunTime = value.LastRunTime;
				//	_GatewaySnapshot.Inputs = value.Inputs;
				_GatewaySnapshot.IsActive = value.IsActive;
				_GatewaySnapshot.Name = value.Name;
				//	_GatewaySnapshot.MockInputs = value.MockInputs;
				_GatewaySnapshot.Order = value.Order;
				_GatewaySnapshot.Query = value.Query;
				//	_GatewaySnapshot.Outputs = value.Outputs;
				_GatewaySnapshot.AutomationId = value.AutomationId;
				_GatewaySnapshot.Type = value.Type;
				_GatewaySnapshot.Script = value.Script;
				_GatewaySnapshot.Status = value.Status;
				_GatewaySnapshot.UpdatedBy = value.UpdatedBy;
				_GatewaySnapshot.UpdatedOn = value.UpdatedOn;
			}
			return _GatewaySnapshot;

		}

		public override GatewayCallerMessage ServiceToBusiness(IEntityTranslatorService service, GatewaySnapshot value)
        {
            GatewayCallerMessage message = null;
            if (value != null)
            {
                message = new GatewayCallerMessage();
                message.EventId = value.EventId;
                message.MessageId = value.Id;
                message.GatewayName = value.GatewayName;
                message.Interval = value.Interval;
                message.CreatedBy = value.CreatedBy;
                message.CreatedOn = value.CreatedOn;
                message.LastRunTime = value.LastRunTime;
                message.IsActive = value.IsActive;
                message.Name = value.Name;
                message.Order = value.Order;
                message.Query = value.Query;
                message.AutomationId = value.AutomationId;
                message.Type = value.Type;
                message.Script = value.Script;
                message.Status = value.Status;
                message.UpdatedBy = value.UpdatedBy;
                message.UpdatedOn = value.UpdatedOn;
            }
            return message;
        }

	}
}
