using Application.Common;
using Application.DTO.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	[Serializable]
	public class ActionTaskCallerMessage : AutomationMessage
	{
		public string ActionId { get; set; }
        public string ActionIdInRunBook { get; set; }
        public string SheetId { get; set; }
        public string Code { get; set; }
        public DictionaryWithDefault<string,dynamic> Inputs { get; set; }
	}
}
