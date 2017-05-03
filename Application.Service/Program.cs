using Application.Messages;
using Application.Utility.IoC.Windsor;
using EasyNetQ;
using RemoteWorker.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.BootstrapStructureMap();
            Resolver.Resolve<IBusBootstrapper>().start();

            Console.ReadKey();
        }
    }
}
