using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
	public interface IActionTaskManager
	{
		ActionTaskMessage GetbyId(string Id);

		ActionTaskMessage Save(ActionTaskMessage actiontaskMessage);
		IEnumerable<ActionTasklist> Get();

        bool executeCode(ActionTaskMessage actiontaskmessage);

    }
}
