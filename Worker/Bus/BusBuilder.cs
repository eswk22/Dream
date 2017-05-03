using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Worker
{
    public class BusBuilder
    {
        public static IBus CreateMessageBus()
        {
            var connectionString =  ConfigurationManager.ConnectionStrings["rabbitmqcon"];
            if (connectionString == null || connectionString.ConnectionString == string.Empty)
            {
                throw new Exception("easynetq connection string is missing or empty");
            }

            return RabbitHutch.CreateBus(connectionString.ConnectionString);
        }
    }
}