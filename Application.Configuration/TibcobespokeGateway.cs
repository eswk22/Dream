using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class TibcobespokeGateway
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
        public bool uppercase { get; set; }
        public int processminthread { get; set; }
        public int processmaxthread { get; set; }
        public int rabbitcorepoolsize { get; set; }
        public int rabbitmaxpoolsize { get; set; }
        public int rabbitkeepalivetime { get; set; }
        public int rabbitblockingqueuesize { get; set; }
        public string tomid { get; set; }
        public string credential { get; set; }
        public string handlerclassname { get; set; }
        public string logtypes { get; set; }
        public string busjarfiles { get; set; }
        public string ldlibrarypath { get; set; }
    }

}
