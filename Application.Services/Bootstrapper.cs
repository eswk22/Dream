using Application.Manager;
using Application.Repository;
using Application.Utility.IoC.Windsor;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	static class Bootstrapper
	{
		public static void BootstrapStructureMap()
		{
			Registrar.Register(typeof(IRepository<>), typeof(MongoRepository<>));

			Registrar.Register<IActionTaskManager, ActionTaskManager>(CrucialLifestyleType.Transient);

			//Repository
			Registrar.Register<IActionTaskRepository, ActionTaskRepository>(CrucialLifestyleType.Transient);


		}
	}
}
