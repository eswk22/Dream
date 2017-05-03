namespace ExecutionEngine.Common.Connect
{
    public class DB2DBConnect
    {
        public static string getConnectURL(string hostname, string dbname)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 50000);
            return getConnectURL(dbHost.host, dbHost.port, dbname);
        }
        public static string getConnectURL(string hostname, int port, string dbname)
        {
            return "jdbc:db2://" + hostname + ":" + port + "/" + dbname;
        }
        public static string Driver
        {
            get
            {
                return "com.ibm.db2.jcc.DB2Driver";
            }
        }
    }
}