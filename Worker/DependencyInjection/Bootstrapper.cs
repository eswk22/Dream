using Application.DTO.Conversion;
using Application.Manager;
using Application.Manager.Implementation;
using Application.Snapshot;
using Application.Utility.IoC.Windsor;
using Application.Utility.Logging;
using Application.Utility.Translators;
using EasyNetQ;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.AutomationHandlers;
using Worker.Bus;
using Application.DAL.ElasticRepository;
using Application.Manager.ServiceContract;

namespace Worker.DependencyInjection
{
	static class Bootstrapper
	{
		public static void BootstrapStructureMap()
		{

			
			Registrar.RegisterCompositionRoot();
            Compiler.Core.Bootstrapper.BootstrapStructureMap();

            //Elastic
            Registrar.Register<IElasticCore, ElasticCore>(CrucialLifestyleType.Singleton);
            Registrar.Register(typeof(IElasticRepository<>), typeof(ElasticRepository<>), CrucialLifestyleType.Transient);

            Registrar.Register<IBusBootstrapper, BusBootstrapper>(CrucialLifestyleType.Singleton);
            Registrar.Register<IRunbookHandler, RunbookHandler>(CrucialLifestyleType.Transient);

            //Compiler.Core.Bootstrapper.BootstrapStructureMap();
            Registrar.Register(typeof(IRepository<>), typeof(MongoRepository<>),CrucialLifestyleType.Transient);
			Registrar.Register<IRepository<ActionTaskSnapshot>, MongoRepository<ActionTaskSnapshot>>(CrucialLifestyleType.Transient);
            Registrar.Register<IRepository<GatewaySnapshot>, MongoRepository<GatewaySnapshot>>(CrucialLifestyleType.Transient);

            Registrar.Register<IAutomationBusinessManager,IAutomationServiceManager, AutomationManager>(CrucialLifestyleType.Transient);
            Registrar.Register<IActionTaskServiceManager, ActionTaskManager>(CrucialLifestyleType.Transient);
            Registrar.Register<IBusinessGatewayManager, GatewayManager>(CrucialLifestyleType.Transient);
            Registrar.Register<ICompileManager, CompileManager>(CrucialLifestyleType.Transient);
            Registrar.Register<IWorksheetServiceManager, WorksheetManager>(CrucialLifestyleType.Transient);
			//Repository
			  Registrar.Register<ILogger, CrucialLogger>(CrucialLifestyleType.Singleton);
            //Handlers

            Registrar.Register<IActionTaskHandler, ActionTaskHandler>(CrucialLifestyleType.Singleton);
            Registrar.Register<IConditionHandler, ConditionHandler>(CrucialLifestyleType.Singleton);


            Registrar.Register<IEntityTranslatorService, EntityTranslatorService>(CrucialLifestyleType.Singleton);
            Registrar.RegisterUsingFactoryMethod<IBus>(BusBuilder.CreateMessageBus,CrucialLifestyleType.Singleton);
            RegisterTranslators(Resolver.Resolve<IEntityTranslatorService>());

    

		}

		private static void RegisterTranslators(IEntityTranslatorService translatorService)
		{
			translatorService.RegisterEntityTranslator(new AutomationTranslator());
			//translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
			translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
			translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());
            translatorService.RegisterEntityTranslator(new GatewayTranslator());
            translatorService.RegisterEntityTranslator(new RemoteTaskTranslator());
            translatorService.RegisterEntityTranslator(new ActionTaskResponseTranslator());

        }



	}
}
