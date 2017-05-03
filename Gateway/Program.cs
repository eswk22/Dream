using Quartz;
using Quartz.Impl;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Configuration;
using Gateway.DependencyInjection;
using Application.Utility.IoC.Windsor;
using EasyNetQ;
using Application.DTO.Gateway;
using Application.Manager;
using Gateway.Bus;

namespace Gateway
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
