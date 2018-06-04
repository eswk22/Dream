using Application.Manager;
using Application.Messages;
using Application.WebApi.Models;
using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi
{
	public class CompilerController : ApiController
	{

		private readonly ICompileManager _CompileManager;
        public CompilerController(ICompileManager compileManager)
		{
			_CompileManager = compileManager;
		}

		[HttpPost]
		[Route("api/compilation")]
		public HttpResponseMessage  Compilation([FromBody] CompilationArguments arguments)
		{
            CompilationResult result = _CompileManager.Compile(arguments);
            return Request.CreateResponse<CompilationResult>(HttpStatusCode.OK, result);
		}

		[HttpPost]
		[Route("api/execute")]
		public object execute([FromBody] CompilationArguments arguments)
		{
			ActionTaskCallerMessage message = new ActionTaskCallerMessage()
			{
				ActionTaskId = "57bc819d9a8c482ee0f162d6"
			};
			return _CompileManager.execute(message);
		}

	}
}
