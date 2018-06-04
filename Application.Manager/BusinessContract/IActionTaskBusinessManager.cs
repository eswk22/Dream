using Application.DTO;
using Application.DTO.ActionTask;
using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Common;

namespace Application.Manager
{
    public interface IActionTaskBusinessManager
    {
        ActionTaskDTO GetbyId(string Id);
        string Add(ActionTaskDTO ActionTaskmessage);
        ActionTaskDTO Update(ActionTaskDTO ActionTaskmessage);
        IEnumerable<ActionTaskDTO> Get();
        ActionTaskDTO Save(ActionTaskDTO ActionTaskmessage,string comment = "");
        ActionTaskDTO Publish(ActionTaskDTO ActionTaskmessage,string comment);
        ActionTaskDTO Delete(ActionTaskDTO ActionTaskmessage);
        ActionTaskDTO Delete(string Id);
        ActionTaskDTO Copy(string Id, string name);
        bool ExistsByName(string name);
        ActionTaskDTO Edit(string actionTaskId,string name, string ATnamespace);
        IEnumerable<ActionTaskDTO> Search(string quickFilter, int page, int size, string sort, FilterDTO[] filterPerColumn,ref int rowCount);
    }
}
