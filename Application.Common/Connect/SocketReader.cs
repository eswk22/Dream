using System;
using System.Collections;
using System.Collections.Generic;
namespace ExecutionEngine.Common.Connect
 {
		 internal class SocketReader : AbstractConnectReader
	 {	/* 28 */	   internal IList<sbyte?> bytes = new ArrayList();
		   internal System.IO.Stream @in;
		   public SocketReader(System.IO.Stream @in)
	   {	/* 33 */		 this.@in = @in;
	/* 35 */		 Daemon = true;
	   }	  
		   public virtual void run()
	   {	/* 40 */		 sbyte[] buff = new sbyte[32768];
			 try
		 {			   for (;;)
		   {	/* 46 */			 int len = this.@in.Read(buff, 0, buff.Length);
	/* 47 */			 if (len == -1)
			 {	/* 49 */			   return;
			 }	/* 51 */			 lock (this)
			 {	
	/* 54 */			   foreach (sbyte byt in buff)
			   {	/* 56 */				 this.bytes.Add(Convert.ToSByte(byt));
			   }	
			 }	
		   }			   return;
		 }			 catch (Exception)
		 {
		 }
	   }	  
		   public virtual string read()
	   {		   lock (this)
		   {
		/* 67 */			 return StringHelperClass.NewString(readByte());
		   }
	   }	  
		   public virtual sbyte[] readByte()
	   {		   lock (this)
		   {
		/* 73 */			 if (this.bytes.Count > 0)
			 {		
		/* 76 */			   sbyte[] result = new sbyte[this.bytes.Count];
		/* 77 */			   int i = 0;
		/* 78 */			   for (IEnumerator i$ = this.bytes.GetEnumerator(); i$.MoveNext();)
			   {
				   sbyte byt = ((sbyte?)i$.Current).Value;
		/* 80 */				 result[(i++)] = byt;
			   }		/* 82 */			   this.bytes.Clear();
		/* 83 */			   return result;
			 }		/* 85 */			 return new sbyte[0];
		   }
	   }	  
		   public virtual void clear()
	   {		   lock (this)
		   {
		/* 90 */			 this.bytes.Clear();
		   }
	   }	  
		   public virtual bool hasContent()
	   {	/* 95 */		 return this.bytes.Count > 0;
	   }	
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\SocketReader.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }