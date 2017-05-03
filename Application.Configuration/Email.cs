using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class Email
    {
        public string Host { get; set; }
        public int SMTP_Port { get; set; }
        public int POP3_Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
