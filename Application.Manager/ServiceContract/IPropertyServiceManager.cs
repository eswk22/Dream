using Application.DTO.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager.ServiceContract
{
    public interface IPropertyServiceManager
    {
        PropertySnapshot GetSnapshotbyId(string Id);
        string Add(PropertySnapshot snapshot);
        PropertySnapshot Update(PropertySnapshot snapshot);


        IEnumerable<PropertySnapshot> GetSnapshots();
        PropertySnapshot Save(PropertySnapshot snapshot);
        PropertySnapshot Delete(PropertySnapshot snapshot);
    }
}
