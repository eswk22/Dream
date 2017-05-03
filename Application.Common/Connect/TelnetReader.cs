using System;
using System.Text;
namespace ExecutionEngine.Common.Connect
 {
		 using Log = com.resolve.util.Log;
				 using Application.Utility.Logging;
		 public class TelnetReader : AbstractConnectReader
	 {	/* 26 */	   internal StringBuilder buffer = new StringBuilder();
		   internal System.IO.Stream @in;
	/* 28 */	   private bool printSpecial = false;
		   public TelnetReader(System.IO.Stream @in, bool printSpecial)
	   {	/* 32 */		 this.@in = @in;
	/* 34 */		 Daemon = true;
	/* 35 */		 this.printSpecial = printSpecial;
	   }	  
	   public virtual bool PrintSpecial
	   {
		   get
		   {		/* 40 */			 return this.printSpecial;
		   }		   set
		   {		/* 45 */			 this.printSpecial = value;
		   }
	   }	  
		   public virtual void run()
	   {			 try
		 {	/* 52 */		   if (this.@in != null)
		   {				 int ch;
	/* 55 */			 while ((ch = this.@in.Read()) != -1)
			 {
	/* 56 */			   if (this.printSpecial)
			   {	
	/* 59 */				 this.buffer.Append((char)ch);
			   }	/* 64 */			   else if (ch < 128)
			   {	/* 66 */				 this.buffer.Append((char)ch);
			   }	
			 }	
		   }	
		 }			 catch (Exception)
		 {
		 }
	   }	  
		   public virtual string read()
	   {		   lock (this)
		   {
		/* 77 */			 string result = "";
		/* 79 */			 if (this.buffer.Length > 0)
			 {		/* 81 */			   result = this.buffer.ToString();
		/* 82 */			   this.buffer = new StringBuilder();
			 }		/* 84 */			 _logger.Debug(result);
		/* 85 */			 Console.WriteLine(result);
		/* 87 */			 return result;
		   }
	   }	  
		   public virtual void clear()
	   {		   lock (this)
		   {
		/* 92 */			 this.buffer = new StringBuilder();
		   }
	   }	  
		   public virtual bool hasContent()
	   {	/* 97 */		 return this.buffer.Length > 0;
	   }	
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\TelnetReader.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }