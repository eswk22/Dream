using Application.DTO.Property;
using Application.Manager.BusinessContract;
using Application.Utility.Translators;
using Application.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Application.WebApi.Controllers
{
    public class PropertyController : ApiController
    {
        private readonly IPropertyBusinessManager _propertyManager;
        private readonly IEntityTranslatorService _translator;

        public PropertyController(IPropertyBusinessManager propertyManager,
            IEntityTranslatorService translatorService)
        {
            _propertyManager = propertyManager;
            _translator = translatorService;
        }


        [HttpGet]
        [Route("get")]
        public PropertyModel GetPropertyById(string Id)
        {
            PropertyModel result = null;
            PropertyDTO dto = _propertyManager.GetbyId(Id);
            result = _translator.Translate<PropertyModel>(dto);
            return result;
        }
        [HttpGet]
        [Route("get")]
        public IList<PropertyModel> GetProperties()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Add")]
        public string Add(PropertyModel model)
        {
            string result = string.Empty;
            PropertyDTO dto = _translator.Translate<PropertyDTO>(model);
            result = _propertyManager.Add(dto);
            return result;
        }

        [HttpPost]
        [Route("Save")]
        public bool Save(PropertyModel model)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route("Delete")]
        public bool Delete(string Id)
        {
            throw new NotImplementedException();
         //   return _propertyManager.Delete(Id);
        }

    }
}
