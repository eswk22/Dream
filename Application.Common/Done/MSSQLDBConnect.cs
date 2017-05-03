namespace ExecutionEngine.Common.Connect
{
    public class MSSQLDBConnect
    {
        public static string getConnectURL(string hostname, string dbname)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 1433);
            return getConnectURL(dbHost.host, dbHost.port, dbname);
        }
        public static string getConnectURL(string hostname, int port, string dbname)
        {
            string url = "jdbc:sqlserver://" + hostname + ":" + port;
            if (!string.ReferenceEquals(dbname, null))
            {
                url = url + ";databaseName=" + dbname;
            }
            return url;
        }
        public static string Driver
        {
            get
            {
                return "com.microsoft.sqlserver.jdbc.SQLServerDriver";
            }
        }
    }
}