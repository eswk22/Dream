using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ActionTask;
using Application.DTO;

namespace Application.Manager
{
	public interface IActionTaskManager
	{
		ActionTaskDTO GetbyId(string Id);

		ActionTaskDTO Save(ActionTaskDTO actiontaskMessage);
		IEnumerable<ActionTasklistDTO> Get();

        bool executeCode(ActionTaskDTO actiontaskmessage);
        ActionTaskMessage GetActionMessagebyId(string actionTaskId);
    }
}
