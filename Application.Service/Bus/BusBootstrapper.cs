using Application.DTO.Gateway;
using Application.Messages;
using Application.Utility.IoC.Windsor;
using EasyNetQ;
using RemoteWorker.ActionTask;
using RemoteWorker.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility = Application.Utility;
using System.Configuration;
using Application.Snapshot;

namespace RemoteWorker.Bus
{
    public class BusBootstrapper : IBusBootstrapper
    {
        private IBus _bus { get; set; }
        public BusBootstrapper(IBus bus)
        {
            _bus = bus;
        }
        public void start()
        {
            string QueueName = getQueuename();

            // Gateway message handler
            _bus.Receive<GatewayCallerMessage>(QueueName, message =>
             {
                 IGateway gateway = Resolver.Resolve<IGateway>();
                 gateway.Execute(message);
             });

            //ActionTask message handler
            _bus.Receive<ActionTaskCallerMessage>(QueueName, message =>
            {
                IActionTaskHandler handler = Resolver.Resolve<IActionTaskHandler>();
                handler.execute(message);
            });
        }
        private string getQueuename()
        {
            string result = ConfigurationManager.AppSettings["Queuename"];
            if (result == null || result == string.Empty)
            {
                throw new Exception("Queue not defined in the app config");
            }
            return result;
        }




    }
}
