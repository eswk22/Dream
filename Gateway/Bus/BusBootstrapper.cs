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
using Utility = Application.Utility;

namespace Gateway.Bus
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

            _bus.Subscribe<GatewayStatusMessage>("GatewayStatusUpdate", message =>
            {
                try
                {
                    IServiceGatewayManager manager = Resolver.Resolve<IServiceGatewayManager>();
                    manager.Update(message);
                }
                catch (Exception ex)
                {
                    // Retry mechanism 
                }
            });
        }





    }
}
