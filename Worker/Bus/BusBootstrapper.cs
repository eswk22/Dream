using Application.DTO.ActionTask;
using Application.DTO.Automation;
using Application.DTO.Gateway;
using Application.Manager;
using Application.Messages;
using Application.Utility.IoC.Windsor;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.AutomationHandlers;
using Utility = Application.Utility;

namespace Worker.Bus
{
    public class BusBootstrapper : IBusBootstrapper
    {
        private IBus _bus { get; set; }
        private string myQueue = "worker";
        public BusBootstrapper(IBus bus)
        {
            _bus = bus;
        }
        public void start()
        {

            //Subscribe messages for Gateway status update 

            _bus.Receive<AutomationMessage>(myQueue, message =>
            {
                try
                {
                    IRunbookHandler handler = Resolver.Resolve<IRunbookHandler>();
                    handler.Execute(message);
                }
                catch (Exception ex)
                {
                    // Retry mechanism 
                }
            });

            _bus.Receive<ActionTaskResponseMessage>(myQueue, message =>
            {
                try
                {
                    IRunbookHandler handler = Resolver.Resolve<IRunbookHandler>();
                    handler.Execute(message);
                }
                catch (Exception ex)
                {
                    // Retry mechanism   
                }
            });


            _bus.Receive<RemoteTaskResponseMessage>(myQueue, message =>
            {
                try
                {
                    IActionTaskHandler handler = Resolver.Resolve<IActionTaskHandler>();
                    handler.Execute(message);
                }
                catch (Exception ex)
                {
                    // Retry mechanism   RemoteTaskResponseMessage
                }
            });

            _bus.Receive<ActionTaskCallerMessage>("worker1", message =>
             {
                 try
                 {
                     IActionTaskHandler handler = Resolver.Resolve<IActionTaskHandler>();
                     handler.Execute(message);
                 }
                 catch (Exception ex)
                 {
                     // Retry mechanism   RemoteTaskResponseMessage
                 }
             });
        }





    }
}
