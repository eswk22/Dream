namespace ExecutionEngine.Common.Connect
 {
		 public class SybaseDBConnect
	 {		   public static string getConnectURL(string hostname)
	   {	/* 31 */		 DBHost dbHost = DBConnectHelper.parseHost(hostname, 7100);
	/* 32 */		 return getConnectURL(dbHost.host, dbHost.port);
	   }	  
		   public static string getConnectURL(string hostname, int port)
	   {	/* 44 */		 return "jdbc:sybase:Tds:" + hostname + ":" + port;
	   }	  
		   public static string getConnectURL(string hostname, string dbname)
	   {	/* 56 */		 DBHost dbHost = DBConnectHelper.parseHost(hostname, 7100);
	/* 57 */		 return getConnectURL(dbHost.host, dbHost.port, dbname);
	   }	  
		   public static string getConnectURL(string hostname, int port, string dbname)
	   {	/* 70 */		 return "jdbc:sybase:Tds:" + hostname + ":" + port + "?ServiceName=" + dbname;
	   }	  
	   public static string Driver
	   {
		   get
		   {		/* 79 */			 return "com.sybase.jdbc3.jdbc.SybDriver";
		   }
	   }	
	 }
 }