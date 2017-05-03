using Application.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Application.Common.Connect
{
    public class SSHConnect
    {
        private ILogger _logger = new CrucialLogger();
        private string _host;
        private int _port;
        private string _username;
        private string _password;
        private string _proxyUsername;
        private string _proxyPassword;
        private int _proxyport;
        private string _proxytype;
        private string _proxyhost;
        private SshClient _client;

        public SshClient Client
        {
            get
            {
                return _client;
            }
            internal set
            {
                _client = value;
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _host = value;
                else
                    throw new Exception("Host can't be null or empty");
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        public int ProxyPort
        {
            get
            {
                return _proxyport;
            }
            set
            {
                _proxyport = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _username = value;
                else
                    throw new Exception("USername can't be null or empty");
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _password = value;
                else
                    throw new Exception("Password can't be null or empty");
            }
        }

        public string ProxyUsername
        {
            get
            {
                return _proxyUsername;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _proxyUsername = value;
                else
                    throw new Exception("Proxy username can't be null or empty");
            }
        }

        public string ProxyPassword
        {
            get
            {
                return _proxyPassword;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _proxyPassword = value;
                else
                    throw new Exception("Proxy password can't be null or empty");
            }
        }

        public string ProxyHost
        {
            get
            {
                return _proxyhost;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _proxyhost = value;
                else
                    throw new Exception("Proxy Host can't be null or empty");
            }
        }

        public string ProxyType
        {
            get
            {
                return _proxytype;
            }
            set
            {
                _proxytype = value;
            }
        }
        public SSHConnect()
        {
        }

        public SSHConnect(SshClient client)
        {
            _client = client;
        }

        public void disconnect()
        {
            try
            {
                if (_client.IsConnected)
                {
                    _client.Disconnect();
                    _client.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        ~SSHConnect()
        {
            this.disconnect();
        }

        public String Execute(string command)
        {
            String result = string.Empty;
            try
            {
                if (_client.IsConnected)
                {
                    SshCommand sshcommand = _client.CreateCommand(command, Encoding.UTF8);
                    result = sshcommand.Execute();

                }
                else
                {
                    throw new Exception("Connection not live");
                }
            }
            catch (SshConnectionException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (SshOperationTimeoutException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            return result;
        }

        public SshClient connect(string hostname, string username, string password, int port)
        {
            try
            {
                this.Host = hostname;
                this.Username = username;
                this.Password = password;
                this.Port = port;
                ConnectionInfo info = new ConnectionInfo(_host, _port, _username,
                    new AuthenticationMethod[]
                    {
                    new PasswordAuthenticationMethod(_username,_password),
                    new PrivateKeyAuthenticationMethod(_username,
                    new PrivateKeyFile[]
                    {
                       new PrivateKeyFile(@"..\openssh.key","string")
                    })
                    });
                return new SshClient(info);
            }
            catch (SshAuthenticationException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (SshOperationTimeoutException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (SshConnectionException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (SshException ex)
            {
                _logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public SshClient connect(string hostname, string username, string password)
        {
            try
            {
                this.Host = hostname;
                this.Username = username;
                this.Password = password;
                ConnectionInfo info = new ConnectionInfo(_host, _username,
                    new AuthenticationMethod[]
                    {
                    new PasswordAuthenticationMethod(_username,_password),
                    new PrivateKeyAuthenticationMethod(_username,
                    new PrivateKeyFile[]
                    {
                       new PrivateKeyFile(@"..\openssh.key","string")
                    })
                    });
                return new SshClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public SshClient connect(string hostname)
        {
            try
            {
                this.Host = hostname;
                ConnectionInfo info = new ConnectionInfo(_host, "anonymous",
                   new AuthenticationMethod[]
                   {
                    new NoneAuthenticationMethod("anonymous")
                   });
                return new SshClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public SshClient Connect()
        {
            try
            {
                if (!string.IsNullOrEmpty(ProxyHost) && !string.IsNullOrEmpty(ProxyPassword))
                {
                    return this.connect(Host, Username, Password, Port, ProxyType, ProxyHost, ProxyPort, ProxyUsername, ProxyUsername);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Password))
                    {
                        if (Port != 0)
                        {
                            return this.connect(Host, Username, Password, Port);
                        }
                        else
                        {
                            return this.connect(Host, Username, Password);
                        }
                    }
                    else
                    {
                        return this.connect(Host);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }
        public SshClient connect(string hostname, string username, string password, int port, string proxytype,
            string proxyhost, int proxyport, string proxyusername, string proxypassword)
        {
            try
            {
                this.Host = hostname;
                this.Username = username;
                this.Password = password;
                this.Port = port;
                this.ProxyHost = proxyhost;
                this.ProxyPassword = proxypassword;
                this.ProxyPort = proxyport;
                this.ProxyType = proxytype;
                ProxyTypes type = ProxyTypes.None;
                switch (ProxyType)
                {
                    case "Http":
                        type = ProxyTypes.Http;
                        break;
                    case "Socks4":
                        type = ProxyTypes.Socks4;
                        break;
                    case "Socks5":
                        type = ProxyTypes.Socks5;
                        break;
                    default:
                        type = ProxyTypes.None;
                        break;
                }

                ConnectionInfo info = new ConnectionInfo(_host, _port, _username,
                   type, _proxyhost, _proxyport, _proxyUsername, _proxyPassword,
                   new AuthenticationMethod[]
                   {
                    new PasswordAuthenticationMethod(_username,_password),
                    new PrivateKeyAuthenticationMethod(_username,
                    new PrivateKeyFile[]
                    {
                       new PrivateKeyFile(@"..\openssh.key","string")
                    })
                   });
                return new SshClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }


    }
}
