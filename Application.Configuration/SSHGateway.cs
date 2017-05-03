﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class SSHGateway
    {
        public bool active { get; set; }
        public string queue { get; set; }
        public int interval { get; set; }
        public int port { get; set; }
        public int maxconnection { get; set; }
        public int timeout { get; set; }
        public int timeoutcounter { get; set; }
    }

}