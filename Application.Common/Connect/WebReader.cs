using System;
using System.Text;
namespace ExecutionEngine.Common.Connect
{
    internal class WebReader : ConnectReader
    {
        internal StringBuilder buffer;
        public WebReader()
        {   /* 26 */
            this.buffer = new StringBuilder();
        }
        public virtual string read()
        {
            lock (this)
            {
                string result = "";
                if (this.buffer.Length > 0)
                {       /* 35 */
                    result = this.buffer.ToString();
                    this.buffer = new StringBuilder();
                }
                return result;
            }
        }
        public virtual void clear()
        {
            lock (this)
            {
                this.buffer = new StringBuilder();
            }
        }
        public virtual bool hasContent()
        {   /* 49 */
            return this.buffer.Length > 0;
        }
        public virtual void set(string contents)
        {   /* 54 */
            this.buffer = new StringBuilder(contents);
        }

        public sbyte[] readByte()
        {
            throw new NotImplementedException();
        }
    }
}