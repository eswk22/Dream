using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	public class ActionTaskResponseMessage
	{
		public string ProblemId { get; set; }
		public string ActionIdInRunBook { get; set; }
		public string ActionTaskId { get; set; }
		public DictionaryWithDefault<string, string> Outputs { get; set; }
		public DictionaryWithDefault<string, string> Results { get; set; }

	}
}
