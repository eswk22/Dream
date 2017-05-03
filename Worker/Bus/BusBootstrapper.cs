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
        public BusBootstrapper(IBus bus)
        {
            _bus = bus;
        }
        public void start()
        {

            //Subscribe messages for Gateway status update 

            _bus.Subscribe<AutomationMessage>("Worker", message =>
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

            _bus.Subscribe<ActionTaskResponseMessage>("Worker", message =>
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


            _bus.Subscribe<RemoteTaskResponseMessage>("Worker", message =>
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

            _bus.Subscribe<ActionTaskCallerMessage>("Worker", message =>
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
