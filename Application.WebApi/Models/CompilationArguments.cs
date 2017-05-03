using Application.Utility;
using Compiler.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApi.Models
{
	public class CompilationArguments1
	{
		[JsonProperty("code")]
		public string Code { get; set; }
		[JsonProperty("mode")]
		public CompilationMode1 Mode { get; set; }
		[JsonProperty("language")]
		public LanguageIdentifier SourceLanguage { get; set; }
		[JsonProperty("target")]
		public LanguageIdentifier TargetLanguage { get; set; }
		[JsonProperty("optimizations")]
		public bool OptimizationsEnabled { get; set; }
	}
}