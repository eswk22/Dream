using Application.DTO.Common;
using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IParameterServiceManager
    {
        ParameterSnapshot GetSnapshotbyId(string Id);
        string Add(ParameterSnapshot Parametermessage);
        ParameterSnapshot Update(ParameterSnapshot snapshot);  
        IEnumerable<ParameterSnapshot> GetSnapshots();
        IEnumerable<ParameterSnapshot> GetSnapshots(string ParentId);
        ParameterSnapshot Save(ParameterSnapshot snapshot);
        ParameterSnapshot Delete(ParameterSnapshot snapshot);
    }
}
