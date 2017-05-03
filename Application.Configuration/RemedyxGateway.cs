using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class RemedyxGateway
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
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public bool poll { get; set; }
        public int pollinterval { get; set; }
        public bool uppercase { get; set; }
    }

}
