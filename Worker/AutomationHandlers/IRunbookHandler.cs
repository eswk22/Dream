using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Automation;
using Application.Messages;
using Application.DTO.ActionTask;

namespace Worker.AutomationHandlers
{
    interface IRunbookHandler
    {
        void Execute(AutomationMessage message);
        void Execute(ActionTaskResponseMessage message);

    }
}
