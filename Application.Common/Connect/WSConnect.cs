using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace ExecutionEngine.Common.Connect
{
    using Log = com.resolve.util.Log;
    using Application.Utility.Logging;
    public class WSConnect
    {
        private string uri;
        private Dictionary<string, string> xmlProperties;
        private URLConnection conn = null;
        public WSConnect(string uri)
        {
            this.uri = uri;
            this.xmlProperties = new Hashtable();
            createDefaultXMLProperties();
        }
        public WSConnect(string uri, Dictionary<string, string> xmlProperties)
        {   
            this.uri = uri;
            this.xmlProperties = xmlProperties;
            createDefaultXMLProperties();
        }
        public virtual void createDefaultXMLProperties()
        {   /*  64 */
            this.xmlProperties["Content-Type"] = "text/xml; charset=utf-8";
        }
        public virtual Dictionary<string, string> Properties
        {
            get
            {       /*  69 */
                return this.xmlProperties;
            }
            set
            {       /*  75 */
                this.xmlProperties = value;
            }
        }
        public virtual string getProperty(string name)
        {   /*  80 */
            return (string)this.xmlProperties[name];
        }
        public virtual void setProperty(string name, string value)
        {   
            this.xmlProperties[name] = value;
        }
        public virtual void connect()
        {
            try
            {   /*  95 */
                URL url = new URL(this.uri);
                this.conn = url.openConnection();
                this.conn.DoOutput = true;
                this.conn.AllowUserInteraction;
                this.conn.addRequestProperty("Content-Type", "text/xml; charset=utf-8");
            }
            catch (MalformedURLException e1)
            {   /* 104 */
                _logger.Error("Invalid URL " + this.uri + ":  " + e1.Message, e1);
                throw e1;
            }
            catch (Exception e2)
            {   /* 109 */
                _logger.Error("Cannot connect to an existing web service" + e2.Message, e2);
                throw e2;
            }
        }
        internal virtual void initConnectionProperties(string xml)
        {   /* 116 */
            IEnumerator<string> propertyEntries = this.xmlProperties.Keys.GetEnumerator();
            while (propertyEntries.MoveNext())
            {   /* 119 */
                string propertyname = (string)propertyEntries.Current;
                if (!propertyname.Equals("Content-Type", StringComparison.CurrentCultureIgnoreCase))
                {   /* 122 */
                    this.conn.addRequestProperty("Content-Type", "text/xml; charset=utf-8");
                    this.xmlProperties["Content-Type"] = "text/xml; charset=utf-8";
                }   /* 125 */
                if (!propertyname.Equals("Content-Length", StringComparison.CurrentCultureIgnoreCase))
                {   /* 127 */
                    this.conn.addRequestProperty("Content-Length", Convert.ToString(xml.Length));
                    this.xmlProperties["Content-Length"] = Convert.ToString(xml.Length);
                }
            }
        }
        public virtual string send(string xml)
        {   /* 136 */
            string response = null;
            initConnectionProperties(xml);
            System.IO.StreamWriter writer = null;
            System.IO.StreamReader reader = null;
            try
            {   /* 146 */
                writer = new System.IO.StreamWriter(this.conn.OutputStream, Encoding.UTF8);
                writer.Write(xml);
                writer.Flush();
                writer.Close();
                reader = new System.IO.StreamReader(this.conn.InputStream);
                response = "\nWebService Response:\n";
                string line = "";
                while (!string.ReferenceEquals((line = reader.ReadLine()), null))
                {   /* 156 */
                    response = response + line + "\n";
                }   /* 158 */
                reader.Close();
                _logger.Trace(response);
            }
            catch (UnsupportedEncodingException e1)
            {   /* 164 */
                _logger.Error("Unfamiliar message content" + e1.Message, e1);
                throw e1;
            }
            catch (IOException e2)
            {   /* 169 */
                _logger.Error("WSConnect.java Caught IOException: " + e2.Message, e2);
                throw e2;
            }
            return response;
        }
    }
}