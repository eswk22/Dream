using System;
 namespace ExecutionEngine.Common.Connect
 {
		 using Log = com.resolve.util.Log;
				 using EchoOptionHandler = org.apache.commons.net.telnet.EchoOptionHandler;
		 using SuppressGAOptionHandler = org.apache.commons.net.telnet.SuppressGAOptionHandler;
		 using TelnetClient = org.apache.commons.net.telnet.TelnetClient;
		 using Application.Utility.Logging;
		 public class TelnetConnect : Connect
	 {	/*  26 */	   protected internal string hostname = null;
		   protected internal int port;
		   protected internal int timeout;
		   protected internal TelnetClient conn;
		   protected internal System.IO.Stream @in;
		   protected internal System.IO.Stream err;
		   protected internal System.IO.Stream @out;
	/*  33 */	   protected internal string charset = null;
	/*  36 */	   private bool printSpecial = false;
	/*  38 *///JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	   protected internal bool sendCarriageReturn_Renamed = false;
//ORIGINAL LINE: public TelnetConnect() throws Exception
	   public TelnetConnect()
	   {
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname) throws Exception
	   public TelnetConnect(string hostname) : this(hostname, 23, 0, null, true, true)
	   {	/*  47 */	
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname, int port) throws Exception
	   public TelnetConnect(string hostname, int port) : this(hostname, port, 0, null, true, true)
	   {	/*  52 */	
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname, int port, int timeout) throws Exception
	   public TelnetConnect(string hostname, int port, int timeout) : this(hostname, port, timeout, null, true, true)
	   {	/*  57 */	
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname, int port, int timeout, String terminalType) throws Exception
	   public TelnetConnect(string hostname, int port, int timeout, string terminalType) : this(hostname, port, timeout, terminalType, true, true)
	   {	/*  62 */	
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname, int port, int timeout, String terminalType, boolean suppressGAOption, boolean echoOption) throws Exception
	   public TelnetConnect(string hostname, int port, int timeout, string terminalType, bool suppressGAOption, bool echoOption) : this(hostname, port, timeout, terminalType, true, true, true)
	   {	/*  67 */	
	   }	   
//ORIGINAL LINE: public TelnetConnect(String hostname, int port, int timeout, String terminalType, boolean suppressGAOption, boolean echoOption, boolean isConnect) throws Exception
	   public TelnetConnect(string hostname, int port, int timeout, string terminalType, bool suppressGAOption, bool echoOption, bool isConnect)
	   {			 try
		 {	/*  75 */		   if (string.ReferenceEquals(terminalType, null))
		   {	/*  77 */			 this.conn = new TelnetClient();
		   }			   else
		   {	/*  81 */			 this.conn = new TelnetClient(terminalType);
		   }	       
			   if (suppressGAOption)
		   {				 SuppressGAOptionHandler gaOpt = new SuppressGAOptionHandler(true, true, true, true);
				 this.conn.addOptionHandler(gaOpt);
		   }	       
	/*  92 */		   if (echoOption)
		   {	/*  94 */			 EchoOptionHandler echoOpt = new EchoOptionHandler(true, true, true, true);
	/*  95 */			 this.conn.addOptionHandler(echoOpt);
		   }	       
	/*  99 */		   if (timeout > 0)
		   {	/* 101 */			 this.conn.ConnectTimeout = timeout;
		   }	       
	/* 104 */		   this.hostname = hostname;
	/* 105 */		   this.port = port;
	/* 106 */		   this.timeout = timeout;
	/* 108 */		   if (isConnect)
		   {	 
	/* 111 */			 this.conn.connect(hostname, port);
	/* 114 */			 this.@in = this.conn.InputStream;
	/* 115 */			 this.@out = this.conn.OutputStream;
	/* 118 */			 this.reader = new TelnetReader(this.@in, this.printSpecial);
	/* 119 */			 ((TelnetReader)this.reader).start();
	/* 121 */			 base.connect();
		   }	
		 }			 catch (Exception e)
		 {	/* 126 */		   _logger.Error("Unable to connect to: " + hostname + " port: " + port + ". " + e.Message);
	/* 127 */		   throw e;
		 }	
	   }	   
	   public virtual bool PrintSpecial
	   {
		   get
		   {		/* 133 */			 return this.printSpecial;
		   }		   set
		   {		/* 142 */			 ((TelnetReader)this.reader).PrintSpecial = value;
		/* 143 */			 this.printSpecial = value;
		   }
	   }	   
	   public virtual string Hostname
	   {
		   get
		   {		/* 148 */			 return this.hostname;
		   }		   set
		   {		/* 153 */			 this.hostname = value;
		   }
	   }	   
	   public virtual int Port
	   {
		   get
		   {		/* 158 */			 return this.port;
		   }		   set
		   {		/* 163 */			 this.port = value;
		   }
	   }	   
	   public virtual int Timeout
	   {
		   get
		   {		/* 168 */			 return this.timeout;
		   }
	   }	   
		   public virtual void close()
	   {			 try
		 {	/* 176 */		   if (this.conn != null)
		   {	/* 178 */			 this.conn.disconnect();
		   }	
		 }			 catch (Exception e)
		 {	/* 183 */		   _logger.Error("Failed to close TelnetConnect: " + e.Message);
		 }	     
	/* 186 */		 this.isClosed = true;
	   }	   
	   public virtual bool Connected
	   {
		   get
		   {		/* 191 */			 bool result = false;
		/* 193 */			 if (this.conn != null)
			 {		/* 195 */			   result = this.conn.Connected;
			 }		     
		/* 198 */			 return result;
		   }
	   }	   
//ORIGINAL LINE: public void flush() throws Exception
	   public virtual void flush()
	   {			 try
		 {	/* 205 */		   if (this.@out != null)
		   {	/* 207 */			 _logger.Trace("flushing");
	/* 208 */			 this.@out.Flush();
		   }	
		 }			 catch (Exception e)
		 {	/* 213 */		   _logger.Warn(e.Message, e);
		 }	
	   }	   
//ORIGINAL LINE: public void send(String line) throws Exception
	   public virtual void send(string line)
	   {	/* 226 */		 send(line, true);
	   }	   
//ORIGINAL LINE: public void send(String line, boolean sendNewline) throws Exception
	   public virtual void send(string line, bool sendNewline)
	   {			 try
		 {	/* 241 */		   base.send();
	/* 244 */		   _logger.Debug("sending: " + line);
	/* 246 */		   if (string.ReferenceEquals(this.charset, null))
		   {	/* 248 */			 _logger.Trace("sending " + line.GetBytes().length + " bytes");
	/* 249 */			 if (sendNewline)
			 {	/* 251 */			   if (this.sendCarriageReturn_Renamed)
			   {	/* 253 */				 line = line + "\r";
			   }	/* 255 */			   line = line + "\n";
			 }	/* 257 */			 this.@out.WriteByte(line.GetBytes());
	/* 258 */			 _logger.Trace("Done Sending Line");
		   }			   else
		   {	 
	/* 266 */			 _logger.Trace("sending " + line.GetBytes().length + " bytes");
	/* 267 */			 this.@out.WriteByte(line.GetBytes(this.charset));
	/* 268 */			 if (sendNewline)
			 {	/* 270 */			   if (this.sendCarriageReturn_Renamed)
			   {	/* 272 */				 this.@out.WriteByte("\r\n".GetBytes(this.charset));
			   }				   else
			   {	/* 276 */				 this.@out.WriteByte("\n".GetBytes(this.charset));
			   }	
			 }	/* 279 */			 _logger.Trace("Done Sending Charset Line");
		   }	/* 281 */		   this.@out.Flush();
	/* 282 */		   _logger.Trace("Output Stream Flush Done");
		 }			 catch (Exception e)
		 {	/* 286 */		   _logger.Warn(e.Message);
	/* 287 */		   throw e;
		 }	
	   }	   
	   public virtual string Charset
	   {
		   set
		   {		/* 293 */			 this.charset = value;
		   }
	   }	   
		   public virtual void sendCarriageReturn(bool sendCarriageReturn)
	   {	/* 298 */		 this.sendCarriageReturn_Renamed = sendCarriageReturn;
	   }	   
	   public virtual TelnetClient TelnetClient
	   {
		   get
		   {		/* 303 */			 return this.conn;
		   }
	   }	
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\TelnetConnect.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }