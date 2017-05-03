using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class HPOMGateway
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
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int socket_timeout { get; set; }
        public bool status_active { get; set; }
        public string status_fieldname { get; set; }
        public int status_process { get; set; }
        public bool runbookid_active { get; set; }
        public string runbookid_fieldname { get; set; }
        public bool uppercase { get; set; }
        public string objects { get; set; }
    }

}
