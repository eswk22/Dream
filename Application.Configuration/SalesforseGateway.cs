﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class SalesforseGateway
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
        public string accounttype { get; set; }
        public string url { get; set; }
        public string httpbasicauthusername { get; set; }
        public string httpbasicauthpassword { get; set; }
        public bool uppercase { get; set; }
        public string objects { get; set; }
    }

}
