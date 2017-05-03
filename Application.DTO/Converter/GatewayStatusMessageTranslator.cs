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
    public class GatewayStatusMessageTranslator : EntityMapperTranslator<GatewayCallerMessage, GatewayStatusMessage>
    {
        public override GatewayStatusMessage BusinessToService(IEntityTranslatorService service, GatewayCallerMessage value)
        {
            GatewayStatusMessage _GatewaySnapshot = null;
			if (value != null)
			{
				_GatewaySnapshot = new GatewayStatusMessage();
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

		public override GatewayCallerMessage ServiceToBusiness(IEntityTranslatorService service, GatewayStatusMessage value)
        {
            GatewayCallerMessage gatewayDTO = null;
            if (value != null)
            {
                gatewayDTO = new GatewayCallerMessage();
                gatewayDTO.EventId = value.EventId;
                gatewayDTO.MessageId = value.Id;
                gatewayDTO.GatewayName = value.GatewayName;
                gatewayDTO.Interval = value.Interval;
                gatewayDTO.CreatedBy = value.CreatedBy;
                gatewayDTO.CreatedOn = value.CreatedOn;
                gatewayDTO.LastRunTime = value.LastRunTime;
                //	gatewayDTO.Inputs = value.Inputs;
                gatewayDTO.IsActive = value.IsActive;
                gatewayDTO.Name = value.Name;
                //	gatewayDTO.MockInputs = value.MockInputs;
                gatewayDTO.Order = value.Order;
                gatewayDTO.Query = value.Query;
                //	gatewayDTO.Outputs = value.Outputs;
                gatewayDTO.AutomationId = value.AutomationId;
                gatewayDTO.Type = value.Type;
                gatewayDTO.Script = value.Script;
                gatewayDTO.Status = value.Status;
                gatewayDTO.UpdatedBy = value.UpdatedBy;
                gatewayDTO.UpdatedOn = value.UpdatedOn;
            }
            return gatewayDTO;
        }

	}
}
