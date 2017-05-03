using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class AmqpGateway
    {
        public bool active { get; set; }
        public string queue { get; set; }
        public bool primary { get; set; }
        public bool secondary { get; set; }
        public bool worker { get; set; }
        public int heartbeat { get; set; }
        public int failover { get; set; }
        public int interval { get; set; }
        public bool uppercase { get; set; }
        public string serverwithport { get; set; }
        public string virtualhost { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool ssl { get; set; }
        public int connectionretry { get; set; }
        public int connectionwait { get; set; }
    }

}
