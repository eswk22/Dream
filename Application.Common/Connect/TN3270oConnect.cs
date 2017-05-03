namespace ExecutionEngine.Common.Connect
 {
		 using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
		 public class TN3270oConnect : OhioConnect, SessionObjectInterface
	 {	
//ORIGINAL LINE: public TN3270oConnect(String host, int port) throws Exception
	   public TN3270oConnect(string host, int port) : base("TN3270", host, port)
	   {	/* 21 */	    
	/* 23 */		 connect();
	   }	
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\TN3270oConnect.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }