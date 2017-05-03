using Application.Manager;
using Application.Manager.Conversion;
using Application.Manager.Implementation;
using Application.Repository;
using Application.Snapshot;
using Application.Utility.IoC.Windsor;
using Application.Utility.Logging;
using Application.Utility.Translators;
using EasyNetQ;
using Gateway.Job;
using MongoRepository;
using Quartz;
using Quartz.Spi;
using Gateway.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Conversion;

namespace Gateway.DependencyInjection
{
	static class Bootstrapper
	{
		public static void BootstrapStructureMap()
		{

			//Registrar.RegisterAllFromAssembliesBasedOn<Profile, DomainToEntityMappingProfile>(CrucialLifestyleType.Transient);

			//var profiles = Registrar.ResolveAll<DomainToEntityMappingProfile>();
			//if (profiles.Length > 0)
			//{
			//	AutoMapperInitializer.Initialize(profiles);
			//}
			Registrar.RegisterCompositionRoot();
            Registrar.Register<IBusBootstrapper, BusBootstrapper>(CrucialLifestyleType.Singleton);

            //Compiler.Core.Bootstrapper.BootstrapStructureMap();
            Registrar.Register(typeof(IRepository<>), typeof(MongoRepository<>));
		    Registrar.Register<IRepository<GatewaySnapshot>, MongoRepository<GatewaySnapshot>>(CrucialLifestyleType.Transient);

            Registrar.Register<IServiceGatewayManager, GatewayManager>(CrucialLifestyleType.Transient);
        
		    Registrar.Register<ILogger, CrucialLogger>(CrucialLifestyleType.Singleton);

			Registrar.Register<IEntityTranslatorService, EntityTranslatorService>(CrucialLifestyleType.Singleton);
            Registrar.RegisterUsingFactoryMethod<IBus>(BusBuilder.CreateMessageBus,CrucialLifestyleType.Singleton);
            RegisterTranslators(Resolver.Resolve<IEntityTranslatorService>());

            Registrar.Register<GatewayScheduler>(CrucialLifestyleType.Transient);

            IJobFactory jobFactory = new WindsorJobFactory(Resolver.Container);
            Registrar.Register<IJobInitializer, JobInitializer>(CrucialLifestyleType.Singleton);
            Registrar.Register(typeof(IJobFactory), jobFactory,CrucialLifestyleType.Singleton);

            Resolver.Resolve<IJobInitializer>().Start();


		}

		private static void RegisterTranslators(IEntityTranslatorService translatorService)
		{
            translatorService.RegisterEntityTranslator(new GatewayCallerMessageTranslator());
            translatorService.RegisterEntityTranslator(new GatewayStatusMsgTranslator());
            //translatorService.RegisterEntityTranslator(new CompilationResultTranslator());
            //translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
            //translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
            //translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());
            translatorService.RegisterEntityTranslator(new Application.DTO.Conversion.GatewayTranslator());

        }



	}
}
