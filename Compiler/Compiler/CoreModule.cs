using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Core.Decompilation;
using Compiler.Core.Processing;
using Compiler.Core.Processing.Languages;
using Compiler.Core.Processing.Languages.Internal;
using Application.Utility.IoC.Windsor;
using Application.Utility;

namespace Compiler.Core
{
	public static partial class Bootstrapper
	{
		public static void BootstrapStructureMap()
		{
			Registrar.Register(typeof(IFeatureDiscovery), typeof(CSharpFeatureDiscovery), LanguageIdentifier.CSharp.ToString(), CrucialLifestyleType.Singleton);
			Registrar.Register(typeof(IFeatureDiscovery), typeof(VBNetFeatureDiscovery), LanguageIdentifier.VBNet.ToString(), CrucialLifestyleType.Singleton);
			Registrar.Register<IRoslynLanguage, VBNetLanguage>(CrucialLifestyleType.Singleton,
			new KeyValuePair<string, object>[1] { new KeyValuePair<string, object>("featureDiscovery", (IFeatureDiscovery)Resolver.Resolve<IFeatureDiscovery>(LanguageIdentifier.VBNet.ToString())) });
			Registrar.Register<IRoslynLanguage, CSharpLanguage>(CrucialLifestyleType.Singleton,
			new KeyValuePair<string, object>[1] { new KeyValuePair<string, object>("featureDiscovery", (IFeatureDiscovery)Resolver.Resolve<IFeatureDiscovery>(LanguageIdentifier.CSharp.ToString())) });
			Registrar.Register<IDecompiler, CSharpDecompiler>(CrucialLifestyleType.Singleton);
			Registrar.Register<IDecompiler, VBNetDecompiler>(CrucialLifestyleType.Singleton);
			Registrar.Register<IDecompiler, ILDecompiler>(CrucialLifestyleType.Singleton);
			Registrar.Register<ICodeProcessor, CodeProcessor>(CrucialLifestyleType.Singleton);
		}
	}

}