using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Automation;

namespace Application.Manager
{
    public interface IAutomationManager
    {
        AutomationEntity GetbyId(string automationId);
    }
}
