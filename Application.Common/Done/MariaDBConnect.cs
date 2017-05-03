namespace ExecutionEngine.Common.Connect
{
    public class MariaDBConnect
    {
        public static string getConnectURL(string hostname, string dbname)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 3306);
            return getConnectURL(dbHost.host, dbHost.port, dbname);
        }
        public static string getConnectURL(string hostname, int port, string dbname)
        {
            return "jdbc:mariadb://" + hostname + ":" + port + "/" + dbname;
        }
        public static string Driver
        {
            get
            {
                return "org.mariadb.jdbc.Driver";
            }
        }
    }
}