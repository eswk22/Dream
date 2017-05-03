using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecuteEngine.Common;
using Application.Configuration;
using Application.Utility.Logging;
using Application.DTO.Gateway;
using System.Data;
using EasyNetQ;
using Application.Utility.Translators;

namespace RemoteWorker.Gateway
{
    public class MSSQLGateway : BaseGateway , IGateway
    {
        private readonly ILogger _logger;
        private IBus _bus;
        private readonly IEntityTranslatorService _translator;
        MSSQLConnector DBConnector { get; set; }

        public MSSQLGateway(ILogger logger,IBus bus, IEntityTranslatorService translator) : base(logger,bus, translator)
        {
            DBConnector = new MSSQLConnector();
            this._logger = logger;
            this._bus = bus;
            this._translator = translator;
        }

        public void Execute(GatewayCallerMessage message)
        {
            try
            {
                if (SetConnectionString(message.GatewayName))
                {
                    DataTable dt = null;
                    try
                    {
                        dt = DBConnector.Select(message.Query);
                    }
                    catch(Exception ex)
                    {
                        base.UpdateGatewayStatus(message, "Failed", "Failed to execute the query");
                        this._logger.Error("Failed to execute the gateway query", ex);
                    }
                    if (dt != null)
                    {
                        base.UpdateGatewayStatus(message, "Executed");
                        foreach (DataRow dr in dt.Rows)
                        {
                            var PARAMS = new Dictionary<string, object>();
                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                PARAMS.Add(dc.ColumnName, dr[dc.ColumnName]);
                            }
                            base.ProcessRecord(PARAMS, message);
                        }
                    }
                }
                else
                {
                    this._logger.Error("Unable to set the connection string for gateway : " + message.GatewayName);
                    base.UpdateGatewayStatus(message,"Failed", "Unable to set the connection string");
                }
            }
            catch(Exception ex)
            {
                this._logger.Error("Unable to execute the gateway", ex, message);
            }
        }

        //private MSSQLGateway GetConfiguration(string gatewayName)
        //{
        //   return Config.Instance.MSSQLGateways.Where(x => x.name == gatewayName && x.active == true).FirstOrDefault();
        //}

        private bool SetConnectionString(string GatewayName)
        {
            bool result = false;
            var gateway = Config.Instance.MSSQLGateways.Where(x => x.name == GatewayName && x.active == true).FirstOrDefault();
            if (gateway != null)
            {
                result = true;
                this.DBConnector.AddConnectionStringParams("Data Source", gateway.host);
                this.DBConnector.AddConnectionStringParams("Persist Security Info", true);
                this.DBConnector.AddConnectionStringParams("User ID", gateway.username);
                this.DBConnector.AddConnectionStringParams("Password", gateway.password);
            }
            else
            {
                _logger.Error("Gateway configurations are not available in the Blueprint. Gateway name" + GatewayName);
            }
            return result;
        }
        private void GetConnector()
        {

        }




    }
}
