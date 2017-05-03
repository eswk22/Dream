using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorker.ActionTask
{
    public interface IActionTaskHandler
    {
        void execute(ActionTaskCallerMessage actiontaskCaller);
    }
}
