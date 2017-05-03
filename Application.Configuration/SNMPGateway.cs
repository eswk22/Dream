using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class SNMPGateway
    {
        public string name { get; set; }
        public bool active { get; set; }
        public string queue { get; set; }
        public bool primary { get; set; }
        public bool secondary { get; set; }
        public bool worker { get; set; }
        public int heartbeat { get; set; }
        public int failover { get; set; }
        public int interval { get; set; }
        public string ipaddress { get; set; }
        public int port { get; set; }
        public string readcommunity { get; set; }
        public string sendtrap_writecommunity { get; set; }
        public int sendtrap_retries { get; set; }
        public int sendtrap_timeout { get; set; }
        public bool uppercase { get; set; }
    }

}
