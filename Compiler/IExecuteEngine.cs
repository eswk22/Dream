using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine
{
	interface IExecuteEngine
	{
		DictionaryWithDefault<string, string> Inputs { get; set; }
		DictionaryWithDefault<string, string> Outputs { get; }
		DictionaryWithDefault<string, string> Results { get; }

		object Execute(string code,string language);
	}
}
