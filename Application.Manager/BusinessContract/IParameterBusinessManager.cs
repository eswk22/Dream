using Application.DTO.Common;
using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IParameterBusinessManager
    {
        ParameterDTO GetbyId(string Id);
        string Add(ParameterDTO parameterdto);
        void Add(string ParentId, IList<ParameterDTO> parameters);
        ParameterDTO Update(ParameterDTO parameterdto);
        void Update(string ParentId, IList<ParameterDTO> parameters);
        IEnumerable<ParameterDTO> GetParameters();
        IEnumerable<ParameterDTO> GetParameters(string ParentId);
        ParameterDTO Save(ParameterDTO parameterdto);
        ParameterDTO Delete(ParameterDTO parameterdto);
        ParameterDTO Delete(string Id);
        void DeletebyParantId(string ParentId);

        bool CopyParameters(string OldParentId, string NewParentId);
    }
}
