using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class PingGateways
    {
        public bool active { get; set; }
        public int interval { get; set; }
        public int timeout { get; set; }
    }

}
