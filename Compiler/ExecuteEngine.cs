using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine
{
	public class ExecuteEngine : IExecuteEngine
	{
		private CSharpScriptEngine _CSharpScriptEngine { get; set; }

		private Globals _globals
		{
			get; set;
		}

		public DictionaryWithDefault<string, string> Inputs
		{
			get
			{
				if (_globals != null)
					return _globals.INPUTS;
				return null;
			}
			set
			{
				_globals = new Globals() { INPUTS = value };
			}
		}

		public DictionaryWithDefault<string, string> Outputs
		{
			get
			{
				if (_globals != null)
					return _globals.OUTPUTS;
				return null;
			}
		}

		public DictionaryWithDefault<string, string> Results
		{
			get
			{
				if (_globals != null)
					return _globals.RESULTS;
				return null;
			}
		}

        public SESSION SESSIONS
        {
            get
            {
                if (_globals != null)
                    return _globals.SESSIONS;
                return null;
            }
        }

        public ExecuteEngine()
		{
			_CSharpScriptEngine = new CSharpScriptEngine();
		}

		public object Execute(string code, string language)
		{
			switch (language)
			{
				case "Csharp":
					return _CSharpScriptEngine.Execute(code, _globals);
				default:
					return null;
			}
		}



	}
	public class Globals
	{
		public DictionaryWithDefault<string, string> INPUTS { get; set; }
		public DictionaryWithDefault<string, string> OUTPUTS { get; set; }
		public DictionaryWithDefault<string, string> RESULTS { get; set; }

        public SESSION SESSIONS { get; set; }

	}
}
