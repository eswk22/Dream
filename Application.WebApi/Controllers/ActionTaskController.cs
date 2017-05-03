using Application.Manager;
using Application.Messages;
using Application.WebApi.Models;
using Compiler.Core;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi
{
    public class ActionTaskController : ApiController
    {

		private readonly IActionTaskManager _ActionTaskManager;
       
		public ActionTaskController(IActionTaskManager actiontaskManager)
		{
			_ActionTaskManager = actiontaskManager;
		}

		[HttpPost]
		[Route("api/actiontask/save")]
		public object Save([FromBody] ActionTaskMessage arguments)
		{
			return _ActionTaskManager.Save(arguments);
		}


		[HttpGet]
		[Route("api/actiontask/get")]
		public object Get()
		{
			return _ActionTaskManager.Get();
		}


        [HttpGet]
        [Route("api/actiontask/getbyid")]
        public object Get(string ActionId)
        {
            return _ActionTaskManager.GetbyId(ActionId);
        }

        [HttpPost]
        [Route("api/actiontask/executecode")]
        public bool ExecuteCode(ActionTaskMessage actiontaskmessage)
        {
            return _ActionTaskManager.executeCode(actiontaskmessage);
        }
    }
}
