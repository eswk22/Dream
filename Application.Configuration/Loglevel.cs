using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class Loglevel
    {
        public string Gateway { get; set; }
        public string Worker { get; set; }
        public string RemoteWorker { get; set; }
        public string Web { get; set; }
    }

}
