using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class EmailGateway
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
        public string protocol { get; set; }
        public bool socialpost { get; set; }
        public string pop3_username { get; set; }
        public string pop3_password { get; set; }
        public string pop3_ipaddress { get; set; }
        public string pop3_port { get; set; }
        public string pop3_folder { get; set; }
        public string pop3_ssl { get; set; }
        public string properties { get; set; }
        public string smtp_host { get; set; }
        public string smtp_port { get; set; }
        public string smtp_from { get; set; }
        public string smtp_username { get; set; }
        public string smtp_password { get; set; }
        public string smtp_ssl { get; set; }
        public string smtp_properties { get; set; }
        public bool uppercase { get; set; }
    }

}
