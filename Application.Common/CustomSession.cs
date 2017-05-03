using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class SESSION
    {
        IDictionary<string, object> _session { get; set; }

        public SESSION()
        {
            _session = new Dictionary<string, object>();
        }

        public object get(string name)
        {
            return _session[name];
        }

        public void set(object value,string name)
        {
            _session.Add(name, value);
        }

        public bool remove(string name)
        {
            return _session.Remove(name);
        }
    }
}
