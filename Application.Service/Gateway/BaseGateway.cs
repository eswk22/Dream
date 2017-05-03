﻿using Application.DTO.Automation;
using Application.DTO.Gateway;
using Application.DTO.RunBook;
using Application.Utility.Logging;
using Application.Utility.Translators;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorker.Gateway
{
    public abstract class BaseGateway
    {
        private readonly ILogger _logger;
        private IBus _bus;
        private readonly IEntityTranslatorService _translator;

        public BaseGateway(ILogger logger, IBus bus, IEntityTranslatorService translator)
        {
            _logger = logger;
            _bus = bus;
            _translator = translator;
        }
        protected void ProcessRecord(Dictionary<string, object> PARAMS, GatewayCallerMessage gateway)
        {
            try
            {
                AutomationMessage message = new AutomationMessage()
                {
                    IncidentId = Guid.NewGuid().ToString(),
                    Parent = null,
                    ParentAutomationFlowId = null,
                    RequestedBy = "System",
                    RequestedOn = DateTime.UtcNow,
                    AutomationId = gateway.AutomationId,
                    ProcessId = Guid.NewGuid().ToString(),
                    Parameters = new AutomationParameter()
                    {
                        Params = PARAMS
                    }
                };
                _bus.Publish<AutomationMessage>(message);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable Send the message to Worker queue", ex);
            }
        }

        internal void UpdateGatewayStatus(GatewayCallerMessage dto, string Status, string ErrorMessage = "")
        {
            GatewayStatusMessage message = _translator.Translate<GatewayStatusMessage>(dto);
            message.Status = Status;
            message.LastRunTime = DateTime.UtcNow;
            _bus.Publish<GatewayStatusMessage>(message);
        }

        protected void GetGatewayJob(string Id)
        {

        }

        private void GetGatewayConfiguration(string Gatewayname)
        {

        }


        private void CallRunbook()
        {

        }
    }
}