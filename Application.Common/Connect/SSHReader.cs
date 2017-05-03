using System;
using System.Text;
namespace ExecutionEngine.Common.Connect
{
    internal class SSHReader : ConnectReader
    {   /* 24 */
        internal StringBuilder buffer = new StringBuilder();
        internal System.IO.Stream @in;
        public SSHReader(System.IO.Stream @in)
        {   /* 29 */
            this.@in = @in;
            /* 32 */
            Daemon = true;
        }
        public SSHReader(string result)
        {   /* 38 */
            this.@in = null;
            /* 41 */
            if (string.ReferenceEquals(result, null))
            {   /* 43 */
                result = "";
            }   /* 45 */
            this.buffer = new StringBuilder(result);
        }
        public virtual void run()
        {   /* 50 */
            sbyte[] buff = new sbyte[32768];
            try
            {   /* 54 */
                while (this.@in != null)
                {   /* 56 */
                    int len = this.@in.Read(buff, 0, buff.Length);
                    /* 57 */
                    if (len == -1)
                    {   /* 59 */
                        return;
                    }
                    /* 63 */
                    lock (this)
                    {   /* 65 */
                        string result = StringHelperClass.NewString(buff, 0, len);
                        /* 66 */
                        result = result.replaceAll("\r\n", System.getProperty("line.separator"));
                        /* 67 */
                        this.buffer.Append(result);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public virtual string read()
        {
            lock (this)
            {
                /* 76 */
                string result = "";
                /* 78 */
                if (this.buffer.Length > 0)
                {       /* 80 */
                    result = this.buffer.ToString();
                    /* 81 */
                    this.buffer = new StringBuilder();
                }
                /* 84 */
                return result;
            }
        }
        public virtual void clear()
        {
            lock (this)
            {
                /* 89 */
                this.buffer = new StringBuilder();
            }
        }
        public virtual bool hasContent()
        {   /* 94 */
            return this.buffer.Length > 0;
        }
    }
    /* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\SSHReader.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
}