using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
	[Serializable]
	public class EmailMessage
	{
		public string To { get; set; }
		public string CC { get; set; }
		public string BCC { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Priority { get; set; }
		public bool IsHTML { get; set; }

	}
}
