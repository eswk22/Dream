using System;
namespace ExecutionEngine.Common.Connect
{
    using StringUtils = com.resolve.util.StringUtils;
    public class PostgreSQLDBConnect
    {
        public static string getConnectURL(string hostname, string dbname)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 5432);
            return getConnectURL(dbHost.host, dbHost.port, dbname);
        }
        public static string getConnectURL(string hostname, int port, string dbname)
        {
            return string.Format("jdbc:postgresql://{0}:{1:D}/{2}", new object[] { hostname, Convert.ToInt32(port), dbname });
        }
        public static string getConnectURL(string hostname, string port, string dbname)
        {
            if (StringUtils.isNumeric(port))
            {
                return getConnectURL(hostname, Convert.ToInt32(port), dbname);
            }
            throw new Exception("Port must be a number like 5432.");
        }
        public static string Driver
        {
            get
            {
                return "org.postgresql.Driver";
            }
        }
    }
}