using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class RemedyGateway
    {
        public string name { get; set; }
        public string queue { get; set; }
        public bool active { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public int maxconnection { get; set; }
        public bool poll { get; set; }
        public int pollinterval { get; set; }
    }

}
