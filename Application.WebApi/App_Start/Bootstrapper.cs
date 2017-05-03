﻿using Application.Manager;
using Application.Manager.Conversion;
using Application.Manager.Implementation;
using Application.Repository;
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

namespace Application.WebApi
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

			Compiler.Core.Bootstrapper.BootstrapStructureMap();

			Registrar.Register(typeof(IRepository<>), typeof(MongoRepository<>));
			Registrar.Register<IRepository<ActionTaskSnapshot>, MongoRepository<ActionTaskSnapshot>>(CrucialLifestyleType.Transient);

			Registrar.Register<IActionTaskManager, ActionTaskManager>(CrucialLifestyleType.Transient);
			Registrar.Register<ICompileManager, CompileManager>(CrucialLifestyleType.Transient);

			//Repository
			Registrar.Register<IActionTaskRepository, ActionTaskRepository>(CrucialLifestyleType.Transient);
			Registrar.Register<ILogger, CrucialLogger>(CrucialLifestyleType.Singleton);

			Registrar.Register<IEntityTranslatorService, EntityTranslatorService>(CrucialLifestyleType.Singleton);
            Registrar.RegisterUsingFactoryMethod<IBus>(BusBuilder.CreateMessageBus,CrucialLifestyleType.Singleton);
            RegisterTranslators(Resolver.Resolve<IEntityTranslatorService>());


		}

		private static void RegisterTranslators(IEntityTranslatorService translatorService)
		{
			translatorService.RegisterEntityTranslator(new CompilationResultTranslator());
			translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
			translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
			translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());

			
		}
	}
}
