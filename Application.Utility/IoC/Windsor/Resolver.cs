using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Application.Utility.IoC.Windsor
{
	public sealed class Resolver
	{
		private static readonly object LockObj = new object();

		private static IWindsorContainer container;

		private static Resolver instance = new Resolver();

		private Resolver()
		{
			container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel,true));
		}

	

		public static IWindsorContainer Container
		{
			get { return container; }

			set
			{
				lock (LockObj)
				{
					container = value;
				}
			}
		}


		internal static Resolver Instance
		{
			get
			{
				if (instance == null)
				{
					lock (LockObj)
					{
						if (instance == null)
						{
							instance = new Resolver();
						}
					}
				}

				return instance;
			}
		}

		public static T Resolve<T>()
		{
			return container.Resolve<T>();
		}

		public static object Resolve(Type type)
		{
			return container.Resolve(type);
		}

		public static object Resolve<T>(string named)
		{
			return container.Resolve<T>(named);
		}
	}
}
