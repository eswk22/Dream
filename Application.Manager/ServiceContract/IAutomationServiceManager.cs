using Application.DTO.Automation;
using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IAutomationServiceManager
    {
        AutomationSnapshot GetSnapshotbyId(string Id);
        string Add(AutomationSnapshot Automationmessage);
        AutomationSnapshot Update(AutomationSnapshot Automationmessage);

    
        IEnumerable<AutomationSnapshot> GetSnapshots();
         AutomationSnapshot Save(AutomationSnapshot Automationmessage);
        AutomationSnapshot Delete(AutomationSnapshot Automationmessage);
    }
}
