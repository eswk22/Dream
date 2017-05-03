using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class XMPPGateway
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
        public string server { get; set; }
        public string port { get; set; }
        public string service { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool sasl { get; set; }
        public string saslmechanism { get; set; }
        public bool uppercase { get; set; }
        public bool socialpost { get; set; }
    }

}
