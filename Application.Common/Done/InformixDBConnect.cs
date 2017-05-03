namespace ExecutionEngine.Common.Connect
{
    public class InformixDBConnect
    {
        public static string getConnectURL(string hostname, string dbname, string servername)
        {   
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 1526);
             return getConnectURL(dbHost.host, dbHost.port, dbname, servername);
        }
        public static string getConnectURL(string hostname, int port, string dbname, string servername)
        {   
            return "jdbc:informix-sqli://" + hostname + ":" + port + "/" + dbname + ":INFORMIXSERVER=" + servername;
        }
        public static string getConnectURL(string hostname, string dbname, string servername, string username, string password)
        {  
            DBHost dbHost = DBConnectHelper.parseHost(hostname, 1526);
            return getConnectURL(dbHost.host, dbHost.port, dbname, servername, username, password);
        }
        public static string getConnectURL(string hostname, int port, string dbname, string servername, string username, string password)
        {  
            return "jdbc:informix-sqli://" + hostname + ":" + port + "/" + dbname + ":INFORMIXSERVER=" + servername + ";user=" + username + ";password=" + password;
        }
        public static string Driver
        {
            get
            {    
                return "com.informix.jdbc.IfxDriver";
            }
        }
    }
}