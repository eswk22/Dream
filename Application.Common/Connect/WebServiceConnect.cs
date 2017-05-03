using System.Threading;
namespace ExecutionEngine.Common.Connect
{
    using WSClient = groovyx.net.ws.WSClient;
    using BasicAuthenticationHelper = groovyx.net.ws.cxf.BasicAuthenticationHelper;
    using MtomHelper = groovyx.net.ws.cxf.MtomHelper;
    using ProxyHelper = groovyx.net.ws.cxf.ProxyHelper;
    using SSLHelper = groovyx.net.ws.cxf.SSLHelper;
    using SoapHelper = groovyx.net.ws.cxf.SoapHelper;
    using Client = org.apache.cxf.endpoint.Client;
    using DynamicClientFactory = org.apache.cxf.endpoint.dynamic.DynamicClientFactory;
    using HTTPConduit = org.apache.cxf.transport.http.HTTPConduit;
    using HTTPClientPolicy = org.apache.cxf.transports.http.configuration.HTTPClientPolicy;
    public class WebServiceConnect : WSClient
    {
        internal const string HTTPS = "https";
        internal bool simpleBindingEnabled = true;
        public WebServiceConnect(string wsdlUrl) : base(wsdlUrl, Thread.CurrentThread.ContextClassLoader)
        {
            initialize();
        }
        public WebServiceConnect(string wsdlUrl, bool? simpleBindingEnabled) : base(wsdlUrl, Thread.CurrentThread.ContextClassLoader)
        {
            this.simpleBindingEnabled = simpleBindingEnabled.Value;
            initialize();
        }
        public virtual bool SimpleBindingEnabled
        {
            get
            {       /*  50 */
                return this.simpleBindingEnabled;
            }
            set
            {       /*  55 */
                this.simpleBindingEnabled = value;
            }
        }
        public virtual void initialize()
        {   /*  68 */
            URL url = this.url;
            this.proxyHelper.initialize();
            this.basicAuthHelper.initialize();
            bool isSSLProtocol = "https".Equals(this.url.Protocol);
            if (isSSLProtocol)
            {   /*  76 */
                this.sslHelper.initialize();
                url = this.sslHelper.getLocalWsdlUrl(this.url);
            }
            this.client = createClient(url);
            this.soapHelper.enable(this.client);
            this.proxyHelper.enable(this.client);
            this.basicAuthHelper.enable(this.client);
            if (isSSLProtocol)
            {   
                this.sslHelper.enable(this.client);
            }
            this.mtomHelper.enable(this.client);
            HTTPConduit conduit = (HTTPConduit)this.client.Conduit;
            configureHttpClientPolicy(conduit);
        }
        private void configureHttpClientPolicy(HTTPConduit conduit)
        {   /* 105 */
            HTTPClientPolicy httpClientPolicy = conduit.Client;
            httpClientPolicy.AllowChunking = false;
            conduit.Client = httpClientPolicy;
        }
        public virtual Client createClient(URL url)
        {   /* 121 */
            Client result = null;
            DynamicClientFactory clientFactory = DynamicClientFactory.newInstance();
            clientFactory.SimpleBindingEnabled = this.simpleBindingEnabled;
            result = clientFactory.createClient(url.toExternalForm(), this.classloader);
            return result;
        }
        public static WSClient createWSClient(string wsdlUrl)
        {   /* 134 */
            WSClient client = new WSClient(wsdlUrl, Thread.CurrentThread.ContextClassLoader);
            client.initialize();
            return client;
        }
        public static object createWSObject(WSClient client, string typeName)
        {   /* 141 */
            return client.create(typeName);
        }
    }
}