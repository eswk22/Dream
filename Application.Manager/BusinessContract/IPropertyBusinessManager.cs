using Application.DTO.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager.BusinessContract
{
    public interface IPropertyBusinessManager
    {
        PropertyDTO GetbyId(string Id);
        string Add(PropertyDTO propertydto);
        PropertyDTO Update(PropertyDTO propertydto);


        IEnumerable<PropertyDTO> GetProperties();
        PropertyDTO Save(PropertyDTO propertydto);
        PropertyDTO Delete(PropertyDTO propertydto);
    }
}
