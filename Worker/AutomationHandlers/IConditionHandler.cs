using Application.DTO.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.AutomationHandlers
{
    public interface IConditionHandler
    {
        string Execute(AutomationParameter parameters, string code);
    }
}
