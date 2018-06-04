using Application.DTO.Automation;
using Application.DTO.Common;
using Application.Manager;
using Application.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi.Controllers
{
    [RoutePrefix("api/automation")]
    public class AutomationController : ApiController
    {
        private readonly IAutomationBusinessManager _automationManager;
        public AutomationController(IAutomationBusinessManager automationManager)
        {
            _automationManager = automationManager;
        }

        [HttpPost]
        [Route("Add")]
        public HttpResponseMessage Add(AutomationDTO dto)
        {
            string result = string.Empty;
            result = _automationManager.Add(dto);
            return Request.CreateResponse<string>(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Update(AutomationDTO dto)
        {
            AutomationDTO result = null;
            result = _automationManager.Update(dto);
            return Request.CreateResponse<AutomationDTO>(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Delete(string Id)
        {
            AutomationDTO result = null;
            result = _automationManager.Delete(Id);
            return Request.CreateResponse<AutomationDTO>(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage GetById(string Id)
        {
            AutomationDTO result = null;
            result = _automationManager.GetbyId(Id);
            return Request.CreateResponse<AutomationDTO>(HttpStatusCode.OK, result);
        }



        public HttpResponseMessage GetDefinedParams(string Id)
        {
            AutomationDTO dto = _automationManager.GetbyId(Id);
            if (dto != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dto.Params);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Not Found");
            }
        }

        public HttpResponseMessage Copy(string Id, string name)
        {
            AutomationDTO dto = null;
            dto = _automationManager.Copy(Id, name);
            return Request.CreateResponse<AutomationDTO>(HttpStatusCode.OK, dto);
        }

        public HttpResponseMessage ExistsByName(string name)
        {
            bool result = false;
            result = _automationManager.ExistsByName(name);
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
            IEnumerable<AutomationDTO> result = null;
            GridResultModel gridResult = null;
            int rowCount = 0;
            result = _automationManager.Search(searchParams.quickFilter, searchParams.page, searchParams.size, searchParams.sort, searchParams.filterPerColumn, ref rowCount);
            if (result != null)
            {
                gridResult = new GridResultModel()
                {
                    data = result,
                    totalElements = rowCount
                };
            }
            return Request.CreateResponse<GridResultModel>(HttpStatusCode.OK, gridResult);
        }


    }
}
