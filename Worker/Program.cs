using Application.Utility.IoC.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Bus;
using Worker.DependencyInjection;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.BootstrapStructureMap();
            Resolver.Resolve<IBusBootstrapper>().start();
        }
    }
}
