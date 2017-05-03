using System;
using System.Net;
namespace ExecutionEngine.Common.Connect
{
    //using BrowserVersion = com.gargoylesoftware.htmlunit.BrowserVersion;
    //using Page = com.gargoylesoftware.htmlunit.Page;
    //using WebClient = com.gargoylesoftware.htmlunit.WebClient;
    //using WebWindow = com.gargoylesoftware.htmlunit.WebWindow;
    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    using Application.Utility.Logging;
    

    public class HtmlConnect : WebClient, SessionObjectInterface
    {
        private ILogger _logger = new CrucialLogger();
        public HtmlConnect()
        {
        }
        public HtmlConnect(BrowserVersion browserVersion) : base(browserVersion)
        {  
        }
        public HtmlConnect(BrowserVersion browserVersion, string proxyHost, int proxyPort) : base(browserVersion, proxyHost, proxyPort)
        {   
        }
        public virtual void setUseInsecureSSLv3Only()
        {   
            base.setUseInsecureSSLv3Only();
        }
        public virtual void close()
        {
            try
            {  
                closeAllWindows();
            }
            catch (Exception e)
            {   
                _logger.Warn("Error closing connection: " + e.Message);
            }
        }
        ~HtmlConnect()
        {
            try
            {   
                closeAllWindows();
            }
            catch (Exception e)
            {   
                _logger.Warn("Error closing connection: " + e.Message);
            }
        }
        public virtual Page CurrentWindowPage
        {
            get
            {     
                Page page = null;
                try
                {      
                    WebWindow window = CurrentWindow;
                    page = window.EnclosedPage;
                }
                catch (Exception e)
                {  
                    _logger.Error("Unable to Retrieve Page: " + e.Message, e);
                }
                return page;
            }
        }
    }
}