using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class ADGateway
    {
        public bool active { get; set; }
        public string queue { get; set; }
        public bool userauth { get; set; }
        public bool grouprequired { get; set; }
    }

}
