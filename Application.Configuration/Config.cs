using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public sealed class Config
    {

        private static volatile Config instance;
        private static object syncRoot = new Object();
        private Config()
        {

        }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            using (StreamReader fs = new StreamReader(@"C:\Eswar\Projects\Dream\Application.Configuration\Config.json", Encoding.UTF8))
                            {
                                instance = JsonConvert.DeserializeObject<Config>(fs.ReadToEnd());
                            }
                        }
                    }
                }

                return instance;
            }
        }

        public string Environment { get; set; }
        public Database Database { get; set; }
        public RabbitMQ RabbitMQ { get; set; }
        public Email Email { get; set; }
        public Loglevel Loglevel { get; set; }
        public Gateways Gateways { get; set; }
        public List<NetcoolGateway> NetcoolGateways { get; set; }
        public List<RemedyGateway> RemedyGateways { get; set; }
        public List<RemedyxGateway> RemedyxGateways { get; set; }
        public List<DBGateway> DBGateways { get; set; }
        public List<EmailGateway> EmailGateways { get; set; }
        public List<TSRMGateway> TSRMGateways { get; set; }
        public List<ITMGateway> ITMGateways { get; set; }
        public List<ExchangeGateway> ExchangeGateways { get; set; }
        public List<XMPPGateway> XMPPGateways { get; set; }
        public List<ServiceNowGateway> ServiceNowGateways { get; set; }
        public List<SalesforseGateway> SalesforseGateways { get; set; }
        public List<SNMPGateway> SNMPGateways { get; set; }
        public List<SSHGateway> SSHGateways { get; set; }
        public List<HPOMGateway> HPOMGateways { get; set; }
        public List<TelnetGateway> TelnetGateways { get; set; }
        public List<EWSGateway> EWSGateways { get; set; }
        public List<LDAPGateway> LDAPGateways { get; set; }
        public List<ADGateway> ADGateways { get; set; }
        public List<HPSMGateway> HPSMGateways { get; set; }
        public List<AmqpGateway> amqpGateways { get; set; }
        public List<TcpGateway> tcpGateways { get; set; }
        public List<HttpGateway> httpGateways { get; set; }
        public List<TibcobespokeGateway> tibcobespokeGateways { get; set; }
        public List<CASpectrumGateway> CASpectrumGateways { get; set; }
        public PingGateways pingGateways { get; set; }

        public List<MSSQLGateway> MSSQLGateways { get; set; }

        public List<PostgreSQLGateway> PostgreSQLGateways { get; set; }
        public List<SybaseGateway> SybaseGateways { get; set; }
        public List<OracleGateway> OracleGateways { get; set; }
        public List<MYSQLGateway> MYSQLGateways { get; set; }
        public List<Db2Gateways> db2Gateways { get; set; }
    }
}
