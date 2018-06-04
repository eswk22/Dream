using Application.DTO;
using Application.DTO.Common;
using Application.Manager;
using Application.Messages;
using Application.WebApi.Models;
using Compiler.Core;
using EasyNetQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi
{
    [RoutePrefix("api/actiontask")]
    public class ActionTaskController : ApiController
    {

        private readonly IActionTaskBusinessManager _actionTaskManager;

        public ActionTaskController(IActionTaskBusinessManager actiontaskManager)
        {
            _actionTaskManager = actiontaskManager;
        }

        [HttpPost]
        [Route("save")]
        public HttpResponseMessage Save([FromBody]ActionTaskDTO dto)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.Save(dto);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);
        }
        [HttpPost]
        [Route("publish")]
        public HttpResponseMessage Publish([FromBody]ActionTaskDTO dto, string comment)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.Publish(dto, comment);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);
        }


        public HttpResponseMessage Update(ActionTaskDTO dto)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.Update(dto);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("DeletebyId")]
        public HttpResponseMessage DeletebyId(string Id)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.Delete(Id);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("GetbyId")]
        public HttpResponseMessage GetbyId(string Id)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.GetbyId(Id);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);
        }

        //public HttpResponseMessage GetDefinedParams(string Id)
        //{
        //    ActionTaskDTO dto = _actionTaskManager.GetbyId(Id);
        //    if (dto != null)
        //    {
        //        ActionTaskDefinedParams param = new ActionTaskDefinedParams()
        //        {
        //            Inputs = dto.p,
        //            Outputs = dto.Outputs
        //        };
        //        return Request.CreateResponse(HttpStatusCode.OK, param);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, "Not Found");
        //    }
        //}

        public HttpResponseMessage Copy(string Id, string name)
        {
            ActionTaskDTO dto = null;
            dto = _actionTaskManager.Copy(Id, name);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, dto);
        }

        public HttpResponseMessage ExistsByName(string name)
        {
            bool result = false;
            result = _actionTaskManager.ExistsByName(name);
            return Request.CreateResponse<bool>(HttpStatusCode.OK, result);
        }
        public HttpResponseMessage GetForDropdown(string name, int size = 15)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("search")]
        public HttpResponseMessage Search(GridSearchDTO searchParams)
        {
            IEnumerable<ActionTaskDTO> result = null;
            GridResultModel gridResult = null;
            int rowCount = 0;
            result = _actionTaskManager.Search(searchParams.quickFilter, searchParams.page, searchParams.size, searchParams.sort, searchParams.filterPerColumn,ref rowCount);
            if (result != null)
            {
                gridResult = new GridResultModel() {
                    data = result,
                    totalElements = rowCount
                };
            }
            return Request.CreateResponse<GridResultModel>(HttpStatusCode.OK, gridResult);
        }

        [HttpGet]
        [Route("edit")]
        public HttpResponseMessage Edit(string ActionTaskId, string name, string ATnamespace)
        {
            ActionTaskDTO result = null;
            result = _actionTaskManager.Edit(ActionTaskId, name, ATnamespace);
            return Request.CreateResponse<ActionTaskDTO>(HttpStatusCode.OK, result);

        }
    }




}


