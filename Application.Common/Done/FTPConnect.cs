using System;
using FluentFTP;
namespace ExecutionEngine.Common.Connect
{
    using ConfigGeneral = com.resolve.rsbase.ConfigGeneral;
    using MainBase = com.resolve.rsbase.MainBase;
    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    //using Log = com.resolve.util.Log;
    using SSLUtils = com.resolve.util.SSLUtils;
    using StringUtils = com.resolve.util.StringUtils;
    using Application.Utility.Logging;
    using System.IO;
    using System.Threading;

    public class FTPConnect : SessionObjectInterface
    {
        static ManualResetEvent m_reset = new ManualResetEvent(false);
        private ILogger _logger = new CrucialLogger();
        private readonly FtpClient client;
        private bool isFTPS = false;
        private bool isFTPES = false;
        private static readonly string RESOLVE_TEMP_DIR = MainBase.main.configGeneral.home + "/tmp/";
        public FTPConnect(string ftpServer, int? port) : this(ftpServer, port, null, null, false, false)
        {
        }
        public FTPConnect(string ftpServer, int? port, string username, string password) : this(ftpServer, port, username, password, false, false)
        {
        }
        public FTPConnect(string ftpServer, int? port, string username, string password, bool isFTPS, bool isFTPES)
        {
            this.isFTPS = isFTPS;
            this.isFTPES = isFTPES;
            SSLSocketFactory sslSocketFactory = null;
            this.client = new FtpClient();
            try
            {
                if (isFTPS)
                {
                    sslSocketFactory = SSLUtils.SSLSocketFactory;
                    this.client.SSLSocketFactory = sslSocketFactory;
                    this.client.SslProtocols = System.Security.Authentication.SslProtocols.Default;
                }
                else if (isFTPES)
                {
                    sslSocketFactory = SSLUtils.SSLSocketFactory;
                    this.client.SSLSocketFactory = sslSocketFactory;
                    this.client.Security = 2;
                }
                this.client.Host = ftpServer;

                if ((port != null) && (port.Value > 0))
                {
                    this.client.Port = port.Value;
                }

                if (StringUtils.isBlank(username))
                {
                    this.client.Credentials = new System.Net.NetworkCredential("anonymous", "resolve");
                }
                else
                {
                    this.client.Credentials = new System.Net.NetworkCredential(username, password);
                }
                this.client.Connect();
            }
            catch (System.InvalidOperationException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpSecurityNotAvailableException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpCommandException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
        }
        public virtual bool FTPS
        {
            get
            {
                return this.isFTPS;
            }
            set
            {
                this.isFTPS = value;
            }
        }
        public virtual bool FTPES
        {
            get
            {
                return this.isFTPES;
            }
            set
            {
                this.isFTPES = value;
            }
        }

        private void BeginSetWorkingDirectoryCallback(IAsyncResult ar)
        {
            try
            {
                FtpClient conn = ar.AsyncState as FtpClient;
                if (conn == null)
                    throw new InvalidOperationException("The FtpControlConnection object is null!");
                conn.EndSetWorkingDirectory(ar);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                m_reset.Set();
            }
        }


        public virtual string download(string directory, string fileToDownload, string localDirectory)//, bool? isBinary)
        {
            string result = null;
            try
            {
                //if (isBinary == null)
                //{
                //    this.client.Type = 0;
                //}
                //else
                //{
                //    this.client.Type = isBinary.Value ? 2 : 1;
                //}
                if (StringUtils.isBlank(localDirectory))
                {
                    localDirectory = RESOLVE_TEMP_DIR;
                }
                else if (!localDirectory.EndsWith("/", StringComparison.Ordinal))
                {
                    localDirectory = localDirectory.Trim() + "/";
                }
                if (this.client.FileExists(directory,
                   FtpListOption.ForceList | FtpListOption.AllFiles))
                {
                    //m_reset.Reset();
                    //this.client.BeginSetWorkingDirectory(directory, new AsyncCallback(BeginSetWorkingDirectoryCallback), this.client);
                    //m_reset.WaitOne();
                    this.client.DownloadFile(localDirectory + fileToDownload, directory + '/' + fileToDownload);
                }
                else
                {
                    throw new Exception("Folder not exist in the FTP");
                }

                result = localDirectory + fileToDownload;

            }
            catch (System.InvalidOperationException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpSecurityNotAvailableException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpCommandException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            return result;
        }
        public virtual bool upload(string directory, string fileToUpload, bool overwrite = false)//bool? isBinary, 
        {
            bool result = false;
            try
            {
                //if (isBinary == null)
                //{
                //    this.client.Type = 0;
                //}
                //else
                //{
                //    this.client.Type = isBinary.Value ? 2 : 1;
                //}

                this.client.UploadFile(fileToUpload, directory, overwrite);
                result = true;
            }
            catch (System.InvalidOperationException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpSecurityNotAvailableException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpCommandException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (FtpException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            return result;
        }
        public virtual void close()
        {
            try
            {
                this.client.Disconnect();
            }
            catch (System.InvalidOperationException e)
            {
                _logger.Error(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
            }
            catch (FtpCommandException e)
            {
                _logger.Error(e.Message, e);
            }
            catch (FtpException e)
            {
                _logger.Error(e.Message, e);
            }
        }

        ~FTPConnect()
        {
            close();
        }
    }
}