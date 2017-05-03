using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class TSRMGateway
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
        public int reconnectdelay { get; set; }
        public int retrydelay { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string httpbasicauthusername { get; set; }
        public string httpbasicauthpassword { get; set; }
        public string url2 { get; set; }
        public string username2 { get; set; }
        public string password2 { get; set; }
        public string httpbasicauthusername2 { get; set; }
        public string httpbasicauthpassword2 { get; set; }
        public bool uppercase { get; set; }
        public string objects { get; set; }
    }

}
