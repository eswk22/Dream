using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Application.Utility.Logging;

namespace Application.Common.Connect
{
    public class HttpConnect
    {
        private HttpClient _client;
        private IDictionary<string, string> _requestHeaders;
        private Uri _Uri;
        private ILogger _logger = new CrucialLogger();
        public HttpConnect()
        {
            _client.BaseAddress = new Uri("");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.Timeout = new TimeSpan(600 * 1000);
        }

        public void AddRequestHeader(string key, string value)
        {
            this._requestHeaders.Add(key, value);
        }

        public void AddDefaultHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }
        public IDictionary<string, string> RequestHeaders
        {
            get
            {
                return _requestHeaders;
            }
        }

        public string Uri
        {
            set
            {
                if (value[0] == '/' || value.StartsWith("http", StringComparison.Ordinal))
                {
                    _Uri = new Uri(value);
                }
                else if (value.StartsWith("www", StringComparison.Ordinal))
                {
                    _Uri = new Uri("http://" + value);
                }
                else
                {
                    throw new Exception("Url is not valid.Url should start with / or http or www");
                }
            }
            get
            {
                return _Uri.AbsoluteUri;
            }
        }


        public Task<HttpResponseMessage> SendAsync(String content)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = _Uri;
            if (_requestHeaders != null)
            {
                foreach (var item in _requestHeaders)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }
            requestMessage.Content = new StringContent(content, Encoding.UTF8);
            return _client.SendAsync(requestMessage);
        }


        public string Send(string Url,String Conent)
        {
            this.Uri = Url;
            return this.Send(Conent);
        }
        public string Send(String content)
        {
            string Response = "";
            try
            {
                this.SendAsync(content).ContinueWith(
                    responseTask =>
                    {
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            Task<string> endResult = result.Content.ReadAsStringAsync();
                            endResult.Wait();
                            if (endResult.IsCompleted)
                            {
                                Response = endResult.Result;
                            }
                            else
                            {
                                _logger.LogException(endResult.Exception);
                                throw endResult.Exception;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(result.ReasonPhrase))
                            {
                                throw new Exception(result.ReasonPhrase);
                            }
                            else
                            {
                                throw new Exception("Unknown exception from server");
                            }
                        }
                    });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
            return Response;
        }

    }
}
