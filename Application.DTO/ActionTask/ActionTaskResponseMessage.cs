using Application.Common;
using Application.DTO.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	public class ActionTaskResponseMessage : AutomationMessage
	{
        public string ActionIdInRunBook { get; set; }
		public string ActionTaskId { get; set; }
    }
}
