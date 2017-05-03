using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	[Serializable]
	public class ActionTaskCallerMessage
	{
		public string ProblemId { get; set; }
		public string ActionIdInRunBook { get; set; }
		public string ActionTaskId { get; set; }
		public DictionaryWithDefault<string, string> Inputs { get; set; }

	}
}
