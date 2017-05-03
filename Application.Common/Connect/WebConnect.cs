using System;
using System.Collections;
namespace ExecutionEngine.Common.Connect
{
    using Log = com.resolve.util.Log;
    using StringUtils = com.resolve.util.StringUtils;
    using DefaultHttpMethodRetryHandler = org.apache.commons.httpclient.DefaultHttpMethodRetryHandler;
    using HttpClient = org.apache.commons.httpclient.HttpClient;
    using NameValuePair = org.apache.commons.httpclient.NameValuePair;
    using StatusLine = org.apache.commons.httpclient.StatusLine;
    using GetMethod = org.apache.commons.httpclient.methods.GetMethod;
    using PostMethod = org.apache.commons.httpclient.methods.PostMethod;
    using HttpMethodParams = org.apache.commons.httpclient.@params.HttpMethodParams;
    using URIUtil = org.apache.commons.httpclient.util.URIUtil;
    using Application.Utility.Logging;
    public class WebConnect : Connect
    {   /*  33 */
        internal bool debug = false;
        internal HttpClient client;
        internal string host;
        internal string uri;
        internal IDictionary @params;
        internal string cookie;
        public WebConnect() : this(false)
        {   /*  43 */
        }
        public WebConnect(bool debug)
        {   /*  48 */
            this.debug = debug;
            this.@params = new Hashtable();
            this.cookie = null;
            this.host = "http://localhost";
            this.uri = "/";
            if (Log.log == null)
            {   /*  57 */
                Log.init("ExecutionEngine.Common.Connect.WebConnect");
            }
            initLogLevel();
            this.client = new HttpClient();
            this.reader = new WebReader();
            base.connect();
        }
        internal virtual void initLogLevel()
        {   /*  72 */
            System.setProperty("org.apache.commons.logging.Log", "org.apache.commons.logging.impl.SimpleLog");
            System.setProperty("org.apache.commons.logging.simplelog.showdatetime", "true");
            if (this.debug)
            {
                System.setProperty("org.apache.commons.logging.simplelog.log.httpclient.wire.header", "debug");
                System.setProperty("org.apache.commons.logging.simplelog.log.org.apache.commons.httpclient", "debug");
            }
            else
            {   
                System.setProperty("org.apache.commons.logging.simplelog.log.org.apache.commons.httpclient", "error");
            }
        }
        public virtual void send(string path)
        {   /*  90 */
            get(path);
        }
        public virtual void get(string path)
        {
            try
            {   /*  97 */
                if (path[0] == '/')
                {   /*  99 */
                    URI = path;
                }   /* 101 */
                else if (path.StartsWith("www", StringComparison.Ordinal))
                {   /* 103 */
                    URL = "http://" + path;
                }   /* 105 */
                else if (path.StartsWith("http", StringComparison.Ordinal))
                {   /* 107 */
                    URL = path;
                }   /* 109 */
                get();
            }
            catch (Exception e)
            {   /* 113 */
                _logger.Error("Failed WebConnect GET request: " + e.Message);
                Console.WriteLine("GET path: " + path + " error: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
        public virtual void get()
        {   /* 123 */
            string url = this.host + URIUtil.encodePathQuery(this.uri);
            _logger.Debug("GET URL: " + url);
            GetMethod method = new GetMethod(url);
            method.Params.setParameter("http.method.retry-handler", new DefaultHttpMethodRetryHandler(3, false));
            if (!StringUtils.isEmpty(this.cookie))
            {   /* 135 */
                method.setRequestHeader("Cookie", this.cookie);
            }
            try
            {   /* 141 */
                int statusCode = this.client.executeMethod(method);
                if (statusCode != 200)
                {   /* 145 */
                    _logger.Debug(method.StatusLine.ToString());
                }
                sbyte[] responseBody = method.ResponseBody;
                ((WebReader)this.reader).set(StringHelperClass.NewString(responseBody));
            }
            catch (Exception e)
            {   /* 156 */
                Console.WriteLine("GET url: " + url + " error: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                _logger.Error("Failed WebConnect GET request: " + e.Message);
            }
            finally
            {   /* 164 */
                method.releaseConnection();
            }
        }
        public virtual void post(string path)
        {
            try
            {   /* 172 */
                if (path[0] == '/')
                {   /* 174 */
                    URI = path;
                }   /* 176 */
                else if (path.StartsWith("www", StringComparison.Ordinal))
                {   /* 178 */
                    URL = "http://" + path;
                }   /* 180 */
                else if (path.StartsWith("http", StringComparison.Ordinal))
                {   /* 182 */
                    URL = path;
                }   /* 184 */
                post();
            }
            catch (Exception e)
            {   /* 188 */
                Console.WriteLine("POST path: " + path + " error: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                _logger.Error("Failed WebConnect POST request: " + e.Message);
            }
        }
        public virtual void post()
        {   /* 198 */
            string url = this.host + URIUtil.encodePathQuery(this.uri);
            _logger.Debug("POST URL: " + url);
            PostMethod method = new PostMethod(url);
            NameValuePair[] data = new NameValuePair[this.@params.Count];
            int idx = 0;
            for (IEnumerator i = this.@params.SetOfKeyValuePairs().GetEnumerator(); i.MoveNext();)
            {   /* 209 */
                DictionaryEntry entry = (DictionaryEntry)i.Current;
                string name = (string)entry.Key;
                string value = (string)entry.Value;
                data[idx] = new NameValuePair(name, value);
                idx++;
            }   /* 216 */
            method.RequestBody = data;
            method.Params.setParameter("http.method.retry-handler", new DefaultHttpMethodRetryHandler(3, false));
            method.Params.setParameter("http.protocol.single-cookie-header", new bool?(true));
            if (!StringUtils.isEmpty(this.cookie))
            {   /* 225 */
                method.setRequestHeader("Cookie", this.cookie);
            }
            try
            {   /* 231 */
                int statusCode = this.client.executeMethod(method);
                if (statusCode != 200)
                {   /* 235 */
                    _logger.Debug(method.StatusLine.ToString());
                }
                sbyte[] responseBody = method.ResponseBody;
                ((WebReader)this.reader).set(StringHelperClass.NewString(responseBody));
            }
            catch (Exception e)
            {   /* 246 */
                Console.WriteLine("POST url: " + url + " error: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                _logger.Error("Failed WebConnect POST request: " + e.Message);
            }
            finally
            {   /* 254 */
                method.releaseConnection();
            }
        }
        public virtual string URL
        {
            set
            {       /* 260 */
                if (!value.StartsWith("http", StringComparison.Ordinal))
                {       /* 262 */
                    value = "http://" + value;
                }
                int pos = value.IndexOf('/', "http://".Length);
                if (pos != -1)
                {       /* 269 */
                    string host = value.Substring(0, pos);
                    string uri = value.Substring(pos, value.Length - pos);
                    Host = host;
                    URI = uri;
                }
                else
                {       /* 276 */
                    Host = value;
                }
            }
        }
        public virtual string Host
        {
            set
            {       /* 282 */
                if (value.StartsWith("http", StringComparison.Ordinal))
                {       /* 284 */
                    this.host = value;
                }
                else
                {       /* 288 */
                    this.host = ("http://" + value);
                }
            }
        }
        public virtual string URI
        {
            set
            {       /* 294 */
                if (value[0] != '/')
                {       /* 296 */
                    value = "/" + value;
                }
                this.uri = value;
            }
        }
        public virtual IDictionary Params
        {
            get
            {       /* 304 */
                return this.@params;
            }
            set
            {       /* 309 */
                this.@params = value;
            }
        }
        public virtual string Cookie
        {
            get
            {       /* 314 */
                return this.cookie;
            }
            set
            {       /* 319 */
                this.cookie = value;
            }
        }
    }
}