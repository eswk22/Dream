using Application.DTO.ActionTask;
using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IActionTaskServiceManager
    {
        ActionTaskSnapshot GetSnapshotbyId(string Id);
        string Add(ActionTaskSnapshot actiontaskSnapshot);
        ActionTaskSnapshot Update(ActionTaskSnapshot actiontaskSnapshot);

    
        IEnumerable<ActionTaskSnapshot> GetSnapshots();
         ActionTaskSnapshot Save(ActionTaskSnapshot actiontaskSnapshot);
        ActionTaskSnapshot Delete(ActionTaskSnapshot actiontaskSnapshot);
    }
}
