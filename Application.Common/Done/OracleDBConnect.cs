namespace ExecutionEngine.Common.Connect
{
    public class OracleDBConnect
    {
        public static string getConnectURL(string hostname, string instance)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 1521);
            return getConnectURL(dbHost.host, dbHost.port, instance);
        }
        public static string getConnectURL(string hostname, int port, string instance)
        {
            return "jdbc:oracle:thin:@" + hostname + ":" + port + ":" + instance;
        }
        public static string getConnectURL(string hostname, string port, string instance)
        {
            return "jdbc:oracle:thin:@" + hostname + ":" + port + ":" + instance;
        }
        public static string Driver
        {
            get
            {
                return "oracle.jdbc.OracleDriver";
            }
        }
    }
}