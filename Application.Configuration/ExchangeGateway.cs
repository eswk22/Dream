using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class ExchangeGateway
    {
        public string name { get; set; }
        public bool active { get; set; }
        public string queue { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public bool ewsmode { get; set; }
        public string ewsurl { get; set; }
        public string mailbox { get; set; }
        public string domain { get; set; }
        public string mapiclientversion { get; set; }
        public int reconnectdelay { get; set; }
        public int retrydelay { get; set; }
        public int maxattachmentmessagesize { get; set; }
        public bool socialpost { get; set; }
    }

}
