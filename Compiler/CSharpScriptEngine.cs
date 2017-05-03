using Compiler;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionEngine
{
	internal class CSharpScriptEngine
	{
		private ScriptState<object> scriptState = null;

		public object ReturnValue
		{
			get
			{
				if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
					return scriptState.ReturnValue;
				return null;
			}
		}

		public ScriptVariable Outputs
		{
			get
			{
				if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
					return scriptState.Variables.Where(x => x.Name == "OUTPUTS").SingleOrDefault();
				return null;
			}
		}

		public ScriptVariable Inputs
		{
			get
			{
				if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
					return scriptState.Variables.Where(x => x.Name == "INPUTS").SingleOrDefault();
				return null;
			}
			set
			{
				if (scriptState.ReturnValue != null)
					scriptState.Variables.Where(x => x.Name == "INPUTS").SingleOrDefault().Value = value;
			}
		}

		public ScriptVariable Results
		{
			get
			{
				if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
					return scriptState.Variables.Where(x => x.Name == "RESULTS").SingleOrDefault();
				return null;
			}
		}

		public object Execute(string code,object globals = null)
		{
			if (scriptState == null)
			{
				var scriptOptions = GetScriptOptions();
				scriptState = CSharpScript.RunAsync(code, options: scriptOptions,globals:globals).Result;
			}
			else
			{
				scriptState = scriptState.ContinueWithAsync(code).Result;
			}
			if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
				return scriptState.ReturnValue;
			return null;			 
		}


		private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();


		private static bool IsSerializable(object value)
		{
			if (value is bool ||
				value is DateTime ||
				value is DateTimeOffset ||
				value is TimeSpan ||
				value is char ||
				value is string ||
				value is decimal ||
				value is byte ||
				value is short ||
				value is int ||
				value is long ||
				value is double ||
				value is float ||
				value is Enum)
			{
				return true;
			}

			try
			{
				using (var memoryStream = new MemoryStream())
				{
					binaryFormatter.Serialize(memoryStream, value);
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		private static ScriptOptions GetScriptOptions()
		{
			return ScriptOptions
				.Default
				.AddReferences(SurfaceArea.GetAssemblies())
				.AddImports(SurfaceArea.GetNamespaces());
		}
	}
}
