using Application.DTO.Automation;
using Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IAutomationBusinessManager
    {
        AutomationDTO GetbyId(string Id);
        string Add(AutomationDTO Automationmessage);
        AutomationDTO Update(AutomationDTO Automationmessage);
        IEnumerable<AutomationDTO> Get();
        AutomationDTO Save(AutomationDTO Automationmessage);
        AutomationDTO Delete(AutomationDTO Automationmessage);
        AutomationDTO Delete(string Id);
        AutomationDTO Copy(string Id, string name);
        IEnumerable<AutomationDTO> Search(string quickFilter, int page, int size, string sort, FilterDTO[] filterPerColumn, ref int rowCount);
        bool ExistsByName(string name);
    }
}
