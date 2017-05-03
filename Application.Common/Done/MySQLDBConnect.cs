namespace ExecutionEngine.Common.Connect
{
    public class MySQLDBConnect
    {
        public static string getConnectURL(string hostname, string dbname)
        {
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 3306);
            return getConnectURL(dbHost.host, dbHost.port, dbname);
        }
        public static string getConnectURL(string hostname, int port, string dbname)
        {
            return "jdbc:mysql://" + hostname + ":" + port + "/" + dbname;
        }
        public static string Driver
        {
            getC:\Eswar\Projects\Dream\Application.Common\Connect\MySQLDBConnect.cs
            {
                return "org.mariadb.jdbc.Driver";
            }
        }
    }
}