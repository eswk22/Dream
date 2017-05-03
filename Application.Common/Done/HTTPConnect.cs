using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
namespace ExecutionEngine.Common.Connect
{
    using Application.Utility.Logging;
    using System.IO;
    using System.Threading.Tasks;

    public class HTTPConnect1 : Connect
    {
        private ILogger _logger = new CrucialLogger();
        internal string uri;
        internal string message;
        internal IDictionary<string, string> Headerproperties;
        public HTTPConnect1()
        {
            this.Headerproperties = new Dictionary<string, string>();
            addProperty("Content-Type", "text/xml; charset=utf-8");
            this.uri = "http://localhost";
            this.message = "";
            this.reader = new WebReader();
            base.connect();
        }
        public HTTPConnect1(IDictionary<string, string> properties)
        {
            this.Headerproperties = properties;
            if (string.ReferenceEquals(properties["Content-Type"], null))
            {
                addProperty("Content-Type", "text/xml; charset=utf-8");
            }
            this.uri = "http://localhost:";
            this.message = "";
            this.reader = new WebReader();
            base.connect();
        }
        public virtual void send(string uri)
        {
            send(uri, null, null);
        }
        public virtual void send(string uri, string message)
        {
            send(uri, message, null);
        }
        public virtual void send(string uri, string message, IDictionary<string, string> properties)
        {
            try
            {
                if (uri[0] == '/')
                {
                    Uri = uri;
                }
                else if (uri.StartsWith("www", StringComparison.Ordinal))
                {
                    Uri = "http://" + uri;
                }
                else if (uri.StartsWith("http", StringComparison.Ordinal))
                {
                    Uri = uri;
                }
                if (string.ReferenceEquals(message, null))
                {
                    message = "";
                }
                message = message.Trim();
                Message = message;
                if (properties != null)
                {
                    foreach (string key in properties.Keys)
                    {
                        addProperty(key, (string)properties[key]);
                    }
                }
                send();
            }
            catch (Exception e)
            {
                _logger.Error("HTTPConnect send fail: " + e.Message, e);
            }
        }
        public virtual void send()
        {
            try
            {
                Uri url = new Uri(this.uri);
                _logger.Debug("Opening Connection: " + this.uri);
                HttpClient conn = new HttpClient();
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = url;
                conn.BaseAddress = url;

                if (this.Headerproperties != null)
                {
                    foreach (string key in this.Headerproperties.Keys)
                    {
                        string value = (string)this.Headerproperties[key];
                        _logger.Trace("Adding Request Property - " + key + ":" + value);
                        requestMessage.Properties.Add(key, value);
                    }
                }
                _logger.Trace("Adding Request Property - Content-Length:" + this.message.Length);
                conn.DefaultRequestHeaders.Add("Content-Length", Convert.ToString(this.message.Length));
                if ((!string.ReferenceEquals(this.message, null)) && (!this.message.Equals("")))
                {
                    this.SendAsync()
                    _logger.Debug("Sending Message: " + this.message);

                }

                _logger.Debug("HTTPConnect Response: " + myStringBuilder.ToString());
                ((WebReader)this.reader).set(myStringBuilder.ToString());
            }
            catch (Exception e)
            {
                _logger.Error("HTTPConnect send fail: " + e.Message, e);
            }
        }

        private async Task<string> SendAsync(HttpClient connection,HttpRequestMessage requestMessage)
        {
            StringBuilder myStringBuilder = new StringBuilder("HTTP Response \n");
            HttpResponseMessage response = await connection.SendAsync(requestMessage);
            _logger.Debug("Getting response");
            if (response.IsSuccessStatusCode)
            {
                Stream br = await response.Content.ReadAsStreamAsync();
                string line = null;
                using (StreamReader reader = new StreamReader(br))
                {
                    while (!string.ReferenceEquals((line = reader.ReadLine()), null))
                    {
                        myStringBuilder.Append(line);
                    }
                }
                br.Close();
            }
            return myStringBuilder.ToString();
        }
        public virtual string Uri
        {
            get
            {
                return this.uri;
            }
            set
            {
                this.uri = value;
            }
        }
        public virtual string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }
        public virtual IDictionary<string, string> Properties
        {
            get
            {
                return this.Headerproperties;
            }
            set
            {
                this.Headerproperties = value;
            }
        }
        public virtual void addProperty(string key, string value)
        {
            this.Headerproperties[key] = value;
        }
        public virtual string getProperty(string key)
        {
            return (string)this.Headerproperties[key];
        }
    }
}