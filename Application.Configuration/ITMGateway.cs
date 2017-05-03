using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class ITMGateway
    {
        public string name { get; set; }
        public bool active { get; set; }
        public string queue { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public string cmd { get; set; }
        public string remote_windows_dir { get; set; }
        public string remote_unix_dir { get; set; }
        public bool deleteoutputs { get; set; }
        public string outputdir { get; set; }
        public string scriptdir { get; set; }
        public int loginduration { get; set; }
        public int timeout { get; set; }
    }

}
