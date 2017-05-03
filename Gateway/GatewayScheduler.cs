using Application.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Configuration;
using Utility = Application.Utility;
using Application.Utility.Logging;
using Quartz;
using EasyNetQ;
using Application.DTO.Gateway;
using Application.Snapshot;
using Application.Utility.Translators;

namespace Gateway
{
    public class GatewayScheduler : IJob
    {
        private IServiceGatewayManager _GatewayManager { get; set; }
        private readonly ILogger _logger;
        private IBus _bus;
        private readonly IEntityTranslatorService _translator;

        public GatewayScheduler(IServiceGatewayManager gatewayManager, ILogger logger,
            IEntityTranslatorService translator,IBus bus)
        {
            _GatewayManager = gatewayManager;
            _logger = logger;
            _bus = bus;
            _translator = translator;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                foreach (var gateway in _GatewayManager.GetSnapshots())//.GetSnapshotsbyInterval(10))
                {
                    if (gateway.Type != null)
                    {
                        if (gateway.Id != null && isGatewayActive(gateway.Type))
                        {
                            string Queuename = GetQueuename(gateway.Type, gateway.GatewayName);
                            if (Queuename != string.Empty)
                            {
                                gateway.Status = "Running";
                                gateway.LastRunTime = DateTime.UtcNow;
                                _GatewayManager.Update(gateway);
                                GatewayCallerMessage gatewayMessage = _translator.Translate<GatewayCallerMessage>(gateway);
                                _bus.Send<GatewayCallerMessage>(Queuename, gatewayMessage);
                            }
                            else
                            {
                                _logger.Error("Queue name is empty for Gateway " + gateway.GatewayName);
                            }
                        }
                        else
                        {
                            _logger.Warn("Gateway is not active", gateway);
                        }
                    }
                    else
                    {
                        _logger.Error("Gateway type can't be null", gateway);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to get the gateways from Database", ex);
            }
        }




        private bool isGatewayActive(string Gatewayname)
        {
            bool result = true;
            var input = (Utility.Gateway)Enum.Parse(typeof(Utility.Gateway), Gatewayname);
            switch (input)
            {
                case Utility.Gateway.caspectrum:
                    result = Config.Instance.Gateways.caspectrum;
                    break;
                case Utility.Gateway.db2:
                    result = Config.Instance.Gateways.db2;
                    break;
                case Utility.Gateway.dbgw:
                    result = Config.Instance.Gateways.dbgw;
                    break;
                case Utility.Gateway.email:
                    result = Config.Instance.Gateways.email;
                    break;
                case Utility.Gateway.emailconnect2:
                    result = Config.Instance.Gateways.emailconnect2;
                    break;
                case Utility.Gateway.ews:
                    result = Config.Instance.Gateways.ews;
                    break;
                case Utility.Gateway.exchange:
                    result = Config.Instance.Gateways.exchange;
                    break;
                case Utility.Gateway.ftpconnect:
                    result = Config.Instance.Gateways.ftpconnect;
                    break;
                case Utility.Gateway.hpom:
                    result = Config.Instance.Gateways.hpom;
                    break;
                case Utility.Gateway.hpsm:
                    result = Config.Instance.Gateways.hpsm;
                    break;
                case Utility.Gateway.htmlconnect:
                    result = Config.Instance.Gateways.htmlconnect;
                    break;
                case Utility.Gateway.http:
                    result = Config.Instance.Gateways.http;
                    break;
                case Utility.Gateway.informix:
                    result = Config.Instance.Gateways.informix;
                    break;
                case Utility.Gateway.itncm:
                    result = Config.Instance.Gateways.itncm;
                    break;
                case Utility.Gateway.mspoi:
                    result = Config.Instance.Gateways.mspoi;
                    break;
                case Utility.Gateway.mssql:
                    result = Config.Instance.Gateways.mssql;
                    break;
                case Utility.Gateway.mysql:
                    result = Config.Instance.Gateways.mysql;
                    break;
                case Utility.Gateway.netcool:
                    result = Config.Instance.Gateways.netcool;
                    break;
                case Utility.Gateway.oracle:
                    result = Config.Instance.Gateways.oracle;
                    break;
                case Utility.Gateway.pdf:
                    result = Config.Instance.Gateways.pdf;
                    break;
                case Utility.Gateway.postgresql:
                    result = Config.Instance.Gateways.postgresql;
                    break;
                case Utility.Gateway.remedy:
                    result = Config.Instance.Gateways.remedy;
                    break;
                case Utility.Gateway.remedyx:
                    result = Config.Instance.Gateways.remedyx;
                    break;
                case Utility.Gateway.salesforce:
                    result = Config.Instance.Gateways.salesforce;
                    break;
                case Utility.Gateway.servicenow:
                    result = Config.Instance.Gateways.servicenow;
                    break;
                case Utility.Gateway.snmp:
                    result = Config.Instance.Gateways.snmp;
                    break;
                case Utility.Gateway.sybase:
                    result = Config.Instance.Gateways.sybase;
                    break;
                case Utility.Gateway.tibcobespoke:
                    result = Config.Instance.Gateways.tibcobespoke;
                    break;
                case Utility.Gateway.tn3270:
                    result = Config.Instance.Gateways.tn3270;
                    break;
                case Utility.Gateway.tn5250:
                    result = Config.Instance.Gateways.tn5250;
                    break;
                case Utility.Gateway.tsrm:
                    result = Config.Instance.Gateways.tsrm;
                    break;
                case Utility.Gateway.vt:
                    result = Config.Instance.Gateways.vt;
                    break;
                case Utility.Gateway.wsliteconnect:
                    result = Config.Instance.Gateways.wsliteconnect;
                    break;
                case Utility.Gateway.xmpp:
                    result = Config.Instance.Gateways.xmpp;
                    break;
            }

            return result;
        }

        private string GetQueuename(string GatewayType, string Gatewayname)
        {
            string Queuename = string.Empty;
            var input = (Utility.Gateway)Enum.Parse(typeof(Utility.Gateway), GatewayType);
            switch (input)
            {
                case Utility.Gateway.caspectrum:
                    CASpectrumGateway gateway = Config.Instance.CASpectrumGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault();
                    Queuename = gateway != null ? gateway.queue : "";
                    break;
                case Utility.Gateway.db2:
                    Queuename = Config.Instance.db2Gateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.dbgw:
                    Queuename = Config.Instance.DBGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.email:
                    Queuename = Config.Instance.EmailGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.ews:
                    Queuename = Config.Instance.EWSGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.exchange:
                    Queuename = Config.Instance.ExchangeGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.hpom:
                    Queuename = Config.Instance.HPOMGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.hpsm:
                    Queuename = Config.Instance.HPSMGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.http:
                    Queuename = Config.Instance.httpGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.itncm:
                    Queuename = Config.Instance.ITMGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.mssql:
                    Queuename = Config.Instance.MSSQLGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.mysql:
                    Queuename = Config.Instance.MYSQLGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.netcool:
                    Queuename = Config.Instance.NetcoolGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.oracle:
                    Queuename = Config.Instance.OracleGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.postgresql:
                    Queuename = Config.Instance.PostgreSQLGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.remedy:
                    Queuename = Config.Instance.RemedyGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.remedyx:
                    Queuename = Config.Instance.RemedyxGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.salesforce:
                    Queuename = Config.Instance.SalesforseGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.servicenow:
                    Queuename = Config.Instance.ServiceNowGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.snmp:
                    Queuename = Config.Instance.SNMPGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.sybase:
                    Queuename = Config.Instance.SybaseGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.tibcobespoke:
                    Queuename = Config.Instance.tibcobespokeGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.tsrm:
                    Queuename = Config.Instance.TSRMGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
                case Utility.Gateway.xmpp:
                    Queuename = Config.Instance.XMPPGateways.Where(x => x.name == Gatewayname && x.active == true).FirstOrDefault().queue;
                    break;
            }
            return Queuename;
        }

    }
}
