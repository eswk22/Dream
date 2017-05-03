
using Application.Snapshot;
using Application.Utility.IoC.Windsor;
using Application.Utility.Logging;
using Application.Utility.Translators;
using EasyNetQ;
using RemoteWorker.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Conversion;
using RemoteWorker.ActionTask;
using RemoteWorker.Bus;

namespace RemoteWorker
{
	static class Bootstrapper
	{
		public static void BootstrapStructureMap()
		{


			Registrar.RegisterCompositionRoot();

            Compiler.Core.Bootstrapper.BootstrapStructureMap();

            Registrar.Register<IGateway, MSSQLGateway>(CrucialLifestyleType.Transient);
            Registrar.Register<IActionTaskHandler, ActionTaskHandler>(CrucialLifestyleType.Transient);
            Registrar.Register<IBusBootstrapper, BusBootstrapper>(CrucialLifestyleType.Singleton);
			Compiler.Core.Bootstrapper.BootstrapStructureMap();

			Registrar.Register<ILogger, CrucialLogger>(CrucialLifestyleType.Singleton);

			Registrar.Register<IEntityTranslatorService, EntityTranslatorService>(CrucialLifestyleType.Singleton);
            Registrar.RegisterUsingFactoryMethod<IBus>(BusBuilder.CreateMessageBus,CrucialLifestyleType.Singleton);
            RegisterTranslators(Resolver.Resolve<IEntityTranslatorService>());


		}

		private static void RegisterTranslators(IEntityTranslatorService translatorService)
		{
			translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
			translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());
            translatorService.RegisterEntityTranslator(new GatewayStatusMessageTranslator());


        }
	}
}
