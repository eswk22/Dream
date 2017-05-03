using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	[Serializable]
	public class ActionTaskMessage
	{
		public string ActionId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Summary { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string module { get; set; }
		public string menupath { get; set; }
		public string Actiontype { get; set; }
		public string Codelanguage { get; set; }
		public string RemoteCode { get; set; }
		public string AccessCode { get; set; }
        public string Queue { get; set; }
        public DictionaryWithDefault<string, dynamic> Inputs { get; set; }
		public DictionaryWithDefault<string, dynamic> Outputs { get; set; }
		public DictionaryWithDefault<string, dynamic> Results { get; set; }
		public DictionaryWithDefault<string, dynamic> MockInputs { get; set; }
		public bool IsActive { get; set; }

		public int TimeOut { get; set; }
	}


}
