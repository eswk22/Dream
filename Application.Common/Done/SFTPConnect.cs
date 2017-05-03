using Application.Utility.Logging;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Connect
{
    public class SFTPConnect
    {

        private ILogger _logger = new CrucialLogger();
        private string _host;
        private int _port;
        private string _username;
        private string _password;
        private SftpClient _client;

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

        public SftpClient Client
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


        public SFTPConnect()
        {

        }

        private void connect()
        {
        }

        public SftpClient connect(string hostname, string username, string password, int port)
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
                return new SftpClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public SftpClient connect(string hostname, string username, string password)
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
                return new SftpClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public SftpClient connect(string hostname)
        {
            try
            {
                this.Host = hostname;
                ConnectionInfo info = new ConnectionInfo(_host, "anonymous",
                   new AuthenticationMethod[]
                   {
                    new NoneAuthenticationMethod("anonymous")
                   });
                return new SftpClient(info);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
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

        ~SFTPConnect()
        {
            this.disconnect();
        }

        public SftpClient Connect()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public void download(string remotefile, string localpath)
        {
            try
            {
                //if (!Directory.Exists(localpath))
                //{
                //    localpath = "applicationtemppath";
                //}
                if (_client.IsConnected)
                {
                    using (var file = File.OpenWrite(localpath))
                    {
                        _client.DownloadFile(remotefile, file);
                    }
                }
                else
                {
                    throw new Exception("Sftp Connection not live");
                    _logger.Error("Sftp Connection not live");
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public void upload(string file, string remotePath)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(file);
                if (fileinfo.Exists)
                {
                    string uploadfile = fileinfo.FullName;
                    if (_client.IsConnected)
                    {
                        var fileStream = new FileStream(uploadfile, FileMode.Open);
                        if (fileStream != null)
                        {
                            _client.UploadFile(fileStream, remotePath);
                        }
                    }
                    else
                    {
                        throw new Exception("Sftp Connection not live");
                        _logger.Error("Sftp Connection not live");
                    }
                }
                else
                {
                    throw new FileNotFoundException();
                    _logger.Error("File Not Found");
                }
            }

            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }


        public void uploadfiles(string[] files, string remotepath)
        {
            foreach (var file in files)
            {

                FileInfo fileinfo = new FileInfo(file);
                if (fileinfo.Exists)
                {
                    string uploadfile = fileinfo.FullName;
                    if (_client.IsConnected)
                    {
                        var fileStream = new FileStream(uploadfile, FileMode.Open);
                        if (fileStream != null)
                        {
                            var UploadResult = _client.BeginUploadFile(fileStream,
                        remoteFile,
                        null,
                        null) as SftpUploadAsyncResult;
                            uploadWaitHandles.Add(testInfo.UploadResult.AsyncWaitHandle);
                        }
                    }
                }
            }

            //  Wait for upload to finish
            bool uploadCompleted = false;
            while (!uploadCompleted)
            {
                //  Assume upload completed
                uploadCompleted = true;

                foreach (var testInfo in testInfoList.Values)
                {
                    var sftpResult = testInfo.UploadResult;

                    if (!testInfo.UploadResult.IsCompleted)
                    {
                        uploadCompleted = false;
                    }
                }
                Thread.Sleep(500);
            }

            //  End file uploads
            foreach (var remoteFile in testInfoList.Keys)
            {
                var testInfo = testInfoList[remoteFile];

                _client.EndUploadFile(testInfo.UploadResult);
                testInfo.UploadedFile.Dispose();
            }

        }
    }
}
