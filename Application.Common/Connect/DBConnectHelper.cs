using System;
namespace ExecutionEngine.Common.Connect
{
    public class DBConnectHelper
    {
        public static DBHost parseHost(string hostName, int defaultPort)
        {
            DBHost dbHost = new DBHost(hostName, defaultPort);
            string[] strings = hostName.Split(":", true);
            if (strings.Length > 2)
            {
                throw new Exception("Invalid host value: " + hostName);
            }
            if (strings.Length == 2)
            {
                try
                {
                    dbHost.host = strings[0];
                    dbHost.port = int.Parse(strings[1]);
                }
                catch (System.FormatException)
                {
                    throw new Exception("Invalid port number : " + strings[1]);
                }
            }
            return dbHost;
        }
    }
}