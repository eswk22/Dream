using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
	public interface ICompileManager
	{
		CompilationResult Compile(CompilationArguments Compileargs);

		ActionTaskResponseMessage execute(ActionTaskCallerMessage actiontaskCaller);
	}
}
