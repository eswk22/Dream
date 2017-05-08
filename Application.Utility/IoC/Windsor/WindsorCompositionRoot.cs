using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Application.Utility.IoC.Windsor
{
	public class WindsorCompositionRoot : IHttpControllerActivator
	{
		private readonly IWindsorContainer container;



        #region constructors
        public WindsorCompositionRoot(IWindsorContainer container, bool performanceLogging = false)
        {

            // Retain a private copy of the kernel.
            this.container = container;
            this.container.Kernel.Resolver.AddSubResolver(
                            new CollectionResolver(container.Kernel, true));
        }



        public WindsorCompositionRoot(IWindsorContainer container, Assembly ass, bool performanceLogging = false)
		{

			// Retain a private copy of the kernel.
			this.container = container;
            this.container.Kernel.Resolver.AddSubResolver(
                            new CollectionResolver(container.Kernel, true));

            container.Register(Classes.FromAssembly(ass)
							.BasedOn<ApiController>()
							.If(t => t.Name.EndsWith("Controller"))
							.Configure(c => c.LifeStyle.Is(LifestyleType.Transient))
							.ConfigureIf(
								c => performanceLogging, c => c.Interceptors("Application.Utility.Interceptors.PerformanceInterceptor")
							)
			);
		}

		public WindsorCompositionRoot(IWindsorContainer container, Assembly ass, List<string> additionalAssemblies, bool performanceLogging = false)
		{
			// Retain a private copy of the kernel.
			this.container = container;

			container.Register(Classes.FromAssembly(ass)
							.BasedOn<ApiController>()
							.If(t => t.Name.EndsWith("Controller"))
							.Configure(c => c.LifeStyle.Is(LifestyleType.Transient))
							.ConfigureIf(
								c => performanceLogging, c => c.Interceptors("Application.Utility.Interceptors.PerformanceInterceptor")
							)
			);

			//now register all controllers from the additional assembly list
			foreach (string assemblyName in additionalAssemblies)
			{
				string path = HttpContext.Current.Server.MapPath("") + "\\bin\\" + assemblyName + ".dll";

				container.Register(Classes.FromAssembly(Assembly.LoadFrom(path))
						 .BasedOn<ApiController>()
						 .If(t => t.Name.EndsWith("Controller"))
						 .Configure(c => c.LifeStyle.Is(LifestyleType.Transient))
						 .ConfigureIf(
							c => performanceLogging, c => c.Interceptors("Application.Utility.Interceptors.PerformanceInterceptor")
						 )
				);
			}
		}

		/// <summary>
		/// Constructs an instance of a WindsorControllerFactory class. 
		/// </summary>
		/// <remarks>This constructor registers all controller types as components.
		/// We need to set up the config, and instruct ASP.Net MVC to use this new controller factory
		/// by calling <code>SetControllerFactory()</code> inside the <code>Application_Start</code> handler in 
		/// Global.asax.cs</remarks>
		/// <param name="container">A Windsor Container instance holding the castle configuration settings.</param>
		//public WindsorCompositionRoot(IWindsorContainer container, bool performanceLogging = false)
		//{
		//	// Retain a private copy of the kernel.
		//	this.container = container;

		//	// new method for MVC3 - used when this class is not in the same assembly as the contollers, controller are assumed to be in the calling assembly...
		//	container.Register(Classes.FromAssembly(Assembly.GetCallingAssembly())
		//					.BasedOn<ApiController>()
		//					.If(t => t.Name.EndsWith("Controller"))
		//					.Configure(c => c.LifeStyle.Is(LifestyleType.Transient))
		//					.ConfigureIf(
		//						c => performanceLogging, c => c.Interceptors("Application.Utility.Interceptors.PerformanceInterceptor")
		//					)
		//	);
		//}

		/// <summary>
		/// Constructs an instance of a WindsorControllerFactory class. (First overload)
		/// </summary>
		/// <param name="container">A Windsor Container instance holding the castle configuration settings.</param>
		/// <param name="additionalAssemblies">A list of assembly names that contain additional controllers.</param>
		public WindsorCompositionRoot(IWindsorContainer container, bool allAssemblies, bool performanceLogging = false): this (container)
        {
			if (allAssemblies)
			{
				container.Register(
					Classes.FromAssemblyInDirectory(
						new AssemblyFilter(HttpRuntime.BinDirectory)
					)
					.BasedOn<ApiController>()
					.Configure(c => c.LifeStyle.Is(LifestyleType.Transient))
					.ConfigureIf(
						c => performanceLogging, c => c.Interceptors("Application.Utility.Interceptors.PerformanceInterceptor")
					)
				);
			}
		}

		#endregion




		public IHttpController Create(
			HttpRequestMessage request,
			HttpControllerDescriptor controllerDescriptor,
			Type controllerType)
		{
			if (controllerType == null)
			{
				throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", ""));
			}
			var controller =
				(IHttpController)this.container.Resolve(controllerType);

			request.RegisterForDispose(
				new Release(
					() => this.container.Release(controller)));

			return controller;
		}

		private class Release : IDisposable
		{
			private readonly Action release;

			public Release(Action release)
			{
				this.release = release;
			}

			public void Dispose()
			{
				this.release();
			}
		}
	}

}
