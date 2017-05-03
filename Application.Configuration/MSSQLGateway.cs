using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class MSSQLGateway
    {
        public string name { get; set; }
        public bool active { get; set; }
        public string queue { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string instance { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isencrypted { get; set; }
    }
}
