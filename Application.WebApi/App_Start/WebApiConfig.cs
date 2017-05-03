using Application.Utility.IoC.StructureMapProvider;
using Application.Utility.IoC.Windsor;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;


namespace Application.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

			config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
			config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
			RegisterFormatters(config);
			// Web API configuration and services
			config.Services.Replace(typeof(IHttpControllerActivator), Registrar.RegisterCompositionRoot());
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

		private static void RegisterFormatters(HttpConfiguration config)
		{
			config.Formatters.Remove(config.Formatters.XmlFormatter);

			var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
			jsonSettings.ContractResolver = new DefaultContractResolver();
			jsonSettings.Converters.Add(new StringEnumConverter());
		}
	}
}
