using Application.Messages;
using Application.Utility.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;
using Application.Snapshot;
using Application.DTO.ActionTask;

namespace Application.DTO.Conversion
{
    public class ActionTaskResponseTranslator : EntityMapperTranslator<ActionTaskCallerMessage, ActionTaskResponseMessage>
    {
        public override ActionTaskResponseMessage BusinessToService(IEntityTranslatorService service, ActionTaskCallerMessage value)
        {
            ActionTaskResponseMessage entity = null;
			if (value != null)
			{
				entity = new ActionTaskResponseMessage();
				entity.ActionIdInRunBook = value.ActionIdInRunBook;
				entity.ActionTaskId = value.ActionTaskId;
				entity.AutomationId = value.AutomationId;
				entity.ConfiguredOutputParams = value.ConfiguredOutputParams;
				entity.IncidentId = value.IncidentId;
				entity.Parameters = value.Parameters;
				entity.Parent = value.Parent;
				entity.ParentAutomationFlowId = value.ParentAutomationFlowId;
				entity.ProcessId = value.ProcessId;
                entity.ActionResultSheetId = value.ActionResultSheetId;
				entity.RequestedBy = value.RequestedBy;
				entity.RequestedOn = value.RequestedOn;
                entity.Inputs = value.Inputs;
                entity.SheetId = value.SheetId;
                entity.Timeout = value.Timeout;
			}
			return entity;

		}

		public override ActionTaskCallerMessage ServiceToBusiness(IEntityTranslatorService service, ActionTaskResponseMessage value)
        {
            ActionTaskCallerMessage entity = null;
            if (value != null)
            {
                entity = new ActionTaskCallerMessage();
                entity.ActionIdInRunBook = value.ActionIdInRunBook;
                entity.ActionTaskId = value.ActionTaskId;
                entity.AutomationId = value.AutomationId;
                entity.ConfiguredOutputParams = value.ConfiguredOutputParams;
                entity.IncidentId = value.IncidentId;
                entity.Parameters = value.Parameters;
                entity.Parent = value.Parent;
                entity.ParentAutomationFlowId = value.ParentAutomationFlowId;
                entity.ProcessId = value.ProcessId;
                entity.RequestedBy = value.RequestedBy;
                entity.RequestedOn = value.RequestedOn;
                entity.Inputs = value.Inputs;
                entity.SheetId = value.SheetId;
                entity.ActionResultSheetId = value.ActionResultSheetId;
                entity.Timeout = value.Timeout;
            }
            return entity;
        }

	}
}
