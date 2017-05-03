using System;
namespace ExecutionEngine.Common.Connect
{
    using Log = com.resolve.util.Log;
    using Application.Utility.Logging;
  
    public class WSLiteConnect
    {
        private static ILogger _logger = new CrucialLogger();
        private readonly string uri;
        private SoapClient client;
        public WSLiteConnect(string uri)
        {
            this.uri = uri;
        }
        public virtual void connect()
        {
            try
            {
                this.client = new SOAPClient(this.uri);
            }
            catch (Exception e2)
            {
                _logger.Error("Cannot connect to an existing web service" + e2.Message, e2);
                throw e2;
            }
        }
        public virtual string send(string soapEnvelope)
        { 
            if (this.client == null)
            {   
                this.client = new SOAPClient(this.uri);
            }
            string result = null;
            try
            {   
                SOAPResponse response = this.client.send(soapEnvelope);
                result = response.Text;
                _logger.Trace(response);
            }
            catch (Exception e)
            {   _logger.Error("WSLiteConnect.java Caught Exception: " + e.Message, e);
                throw e;
            }
            return result;
        }
    }
}