using System;
namespace ExecutionEngine.Common.Connect
{
    using Log = com.resolve.util.Log;
    using StringUtils = com.resolve.util.StringUtils;
    using Application.Utility.Logging;
    using System.Net.Sockets;
    using System.Net;

    public class SocketConnect : Connect
    {
        private ILogger _logger = new CrucialLogger();
        internal Socket conn;
        internal System.IO.Stream @in;
        internal System.IO.Stream err;
        internal System.IO.Stream @out;
        internal string charset = null;
        public SocketConnect(string hostname, int port) : this(hostname, port, 0)
        { 
        }
        public SocketConnect(string hostname, int port, int timeout)
        {
            try
            {  
                this.conn = new Socket(hostname, port);
                SocketAddress address = new InetSocketAddress(hostname, port);
                if (timeout > 0)
                {   
                    this.conn.SoTimeout = timeout;
                }   
                if (!this.conn.Connected)
                {
                    this.conn.connect(address);
                }
                this.@in = this.conn.InputStream;
                this.@out = this.conn.OutputStream;
                this.reader = new SocketReader(this.@in);
                ((SocketReader)this.reader).start();
                base.connect();
            }
            catch (Exception e)
            {  
                _logger.Warn("Unable to connect to: " + hostname + " port: " + port + ". " + e.Message);
            }
        }
        public virtual void close()
        {
            try
            {   
                if (this.conn != null)
                {  
                    this.conn.close();
                }
            }
            catch (Exception e)
            {
                _logger.Error("Failed to close TelnetConnect: " + e.Message);
            }
            this.isClosed = true;
        }
        public virtual bool Connected
        {
            get
            {   
                bool result = false;
                if (this.conn != null)
                {      
                    result = this.conn.Connected;
                }
               
                return result;
            }
        }
        public virtual void flush()
        { 
            if (this.@out != null)
            {  
                _logger.Trace("flushing");
                 this.@out.Flush();
            }
        }
         public virtual void send(sbyte[] data)
        {
            try
            { 
                base.send();
                _logger.Trace("sending " + data.Length + " bytes");
                this.@out.Write(data, 0, data.Length);
                this.@out.Flush();
            }
            catch (Exception e)
            {   
                _logger.Warn(e.Message);
            }
        }
        public virtual void send(string line)
        {   
            if (StringUtils.isNotEmpty(line))
            {   
                _logger.Trace("sending " + line);
                try
                {  
                    base.send();
                    if (string.ReferenceEquals(this.charset, null))
                    {   
                        send(line.GetBytes());
                    }
                    else
                    {   
                        _logger.Trace("sending " + line.GetBytes().length + " bytes");
                        send(line.GetBytes(this.charset));
                    }
                }
                catch (Exception e)
                {   
                    _logger.Warn(e.Message);
                }
            }
        }
        public virtual string Charset
        {
            set
            {       /* 157 */
                this.charset = value;
            }
        }
    }
 
}