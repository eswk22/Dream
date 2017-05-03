using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.RunBook
{
    public class RunBookInitializerMessage
    {
        public Guid Id { get; set; }

        public string RunbookId { get; set; }
        public Dictionary<string, object> PARAMS { get; set; }
    }
}
