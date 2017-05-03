using Application.Manager;
using Application.Utility.Logging;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi
{
    public class DefaultController : ApiController
    {
		private IActionTaskManager _IActionTaskManager;
        private IBus _bus;
		public DefaultController(IActionTaskManager iActionTaskManager,ILogger _logger, IBus bus)
		{                                                                       
			_IActionTaskManager = iActionTaskManager;
            _bus = bus;
			_logger.Error("Test Message");
		}

	
        // GET: api/Default
        public IEnumerable<string> Get()
        {
		   return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
