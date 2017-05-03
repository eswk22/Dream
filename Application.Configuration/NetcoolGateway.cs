using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class NetcoolGateway
    {
        public string name { get; set; }
        public bool active { get; set; }
        public string queue { get; set; }
        public bool primary { get; set; }
        public bool secondary { get; set; }
        public bool worker { get; set; }
        public string ipaddress { get; set; }
        public string ipaddress2 { get; set; }
        public int port { get; set; }
        public int port2 { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool status_active { get; set; }
        public string status_fieldname { get; set; }
        public int status_process { get; set; }
        public bool runbookid_active { get; set; }
        public string runbookid_fieldname { get; set; }
        public int version { get; set; }
        public int poolsize { get; set; }
        public int minevictableidletime { get; set; }
        public int timebetweenevictionruns { get; set; }
        public string url { get; set; }
        public string url2 { get; set; }
        public string driver { get; set; }
        public int heartbeat { get; set; }
        public int failover { get; set; }
        public int reconnectdelay { get; set; }
        public int retrydelay { get; set; }
        public bool uppercase { get; set; }
    }

}
