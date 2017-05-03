namespace ExecutionEngine.Common.Connect
 {
		 using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
		 public class TN5250oConnect : OhioConnect, SessionObjectInterface
	 {	
//ORIGINAL LINE: public TN5250oConnect(String host, int port) throws Exception
	   public TN5250oConnect(string host, int port) : base("TN5250", host, port)
	   {	/* 21 */	    
	/* 23 */		 connect();
	   }	
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\TN5250oConnect.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }