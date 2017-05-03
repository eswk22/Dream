using Application.DTO.ActionTask;
using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.AutomationHandlers
{
    public interface IActionTaskHandler
    {
        void Execute(ActionTaskCallerMessage actiontaskCaller);
        void Execute(RemoteTaskResponseMessage responseMessage);
    }
}
