using System;
using System.Net.Mail;
namespace ExecutionEngine.Common.Connect
{
    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    using Application.Utility.Logging;

    public class EmailConnect : SessionObjectInterface
    {
        private ILogger _logger = new CrucialLogger();
        private const string DEFAULT_CONTENTTYPE = "text/html";
        private SmtpClient Client;
        internal bool isMultipart;
        internal Multipart multipart;
        internal bool secure;
        internal Session session;
        internal MimeMessage message;
        internal string host;
        internal int port;
        internal string username;
        internal string password;
        public EmailConnect(string host, int port) : this(host, port, false)
        {   
        }
        public EmailConnect(string host, int port, bool hasAttachment)
        {
            this.isMultipart = hasAttachment;
            this.secure = false;
            this.host = host;
            this.port = port;
            Properties props = System.Properties;
            props.put("mail.smtp.host", host);
            props.put("mail.smtp.port", Convert.ToString(port));
            props.put("mail.smtp.connectiontimeout", Convert.ToInt32(60000));
            this.session = Session.getDefaultInstance(props, null);
            this.message = new MimeMessage(this.session);
            if (this.isMultipart)
            {   
                this.multipart = new MimeMultipart();
                this.message.Content = this.multipart;
            }
        }
        public EmailConnect(string host, int port, string username, string password) : this(host, port, username, password, false)
        {   
        }
        public EmailConnect(string host, int port, string username, string password, bool hasAttachment)
        {
            this.isMultipart = hasAttachment;
            this.secure = true;
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
            Properties props = System.Properties;
            props.put("mail.smtps.host", host);
            props.put("mail.smtps.port", Convert.ToString(port));
            props.put("mail.smtps.connectiontimeout", Convert.ToInt32(60000));
            props.put("mail.smtps.auth", "true");
            this.session = Session.getDefaultInstance(props, null);
            this.message = new MimeMessage(this.session);
            if (this.isMultipart)
            {  
                this.multipart = new MimeMultipart();
                this.message.Content = this.multipart;
            }
        }
        public virtual void close()
        {
        }
        ~EmailConnect()
        {
        }
        public virtual string send()
        {   
            string result = "";
            Transport transport = null;
            try
            {   
                if (!this.secure)
                {   
                    Transport.send(this.message);
                }
                else
                {   
                    transport = this.session.getTransport("smtps");
                    transport.connect(this.host, this.port, this.username, this.password);
                    transport.sendMessage(this.message, this.message.AllRecipients);
                }
            }
            catch (Exception e)
            {   
                result = "Unable to send message : " + e.Message;
                _logger.Error("Unable to send message: " + e.Message, e);
            }
            finally
            {   
                if (transport != null)
                {   
                    transport.close();
                }
            }   
            return result;
        }
        public virtual string setFrom(string from)
        {   
            string result = "";
            try
            {  
                this.message.From = new InternetAddress(from);
            }
            catch (Exception e)
            {  
                result = "Unable to add recipient: " + e.Message;
                _logger.Error("Unable to add recipient: " + e.Message);
            }
            return result;
        }
        public virtual string setReplyTo(string replyTo)
        {  
            string result = "";
            if (StringUtils.isNotBlank(replyTo))
            {
                try
                {  
                    Address replyToAddress = new InternetAddress(replyTo);
                    this.message.ReplyTo = new Address[] { replyToAddress };
                }
                catch (Exception e)
                {  
                    result = "Unable to add recipient: " + e.Message;
                    _logger.Error("Unable to add recipient: " + e.Message);
                }
            }   
            return result;
        }
        public virtual string addTo(string addr)
        {   
            string result = "";
            try
            {  
                this.message.addRecipient(Message.RecipientType.TO, new InternetAddress(addr));
            }
            catch (Exception e)
            {  
                result = "Unable to add recipient: " + e.Message;
                _logger.Error("Unable to add recipient: " + e.Message);
            }
            return result;
        }
        public virtual string addCC(string addr)
        {   
            string result = "";
            try
            {
                this.message.addRecipient(Message.RecipientType.CC, new InternetAddress(addr));
            }
            catch (Exception e)
            {
                result = "Unable to add recipient: " + e.Message;
                _logger.Error("Unable to add recipient: " + e.Message);
            }
            return result;
        }
        public virtual string addBCC(string addr)
        {   
            string result = "";
            try
            {   
                this.message.addRecipient(Message.RecipientType.BCC, new InternetAddress(addr));
            }
            catch (Exception e)
            {   
                result = "Unable to add recipient: " + e.Message;
                _logger.Error("Unable to add recipient: " + e.Message);
            }   
            return result;
        }
        public virtual string setSubject(string subject)
        {   
            string result = "";
            try
            {   
                this.message.Subject = subject;
            }
            catch (Exception e)
            {   
                result = "Unable to set message subject: " + e.Message;
                _logger.Error("Unable to set message subject: " + e.Message);
            }  
            return result;
        }
        public virtual string setText(string text)
        { 
            return setText(text, "text/html");
        }
        public virtual string setText(string text, string contentType)
        {   
            string result = "";
            try
            {  
                if (this.isMultipart)
                {   
                    BodyPart messageBodyPart = new MimeBodyPart();
                    messageBodyPart.Text = text;
                    this.multipart.addBodyPart(messageBodyPart);
                }
                else
                {   
                    this.message.setContent(text, contentType);
                }
            }
            catch (Exception e)
            {   
                result = "Unable to set message text: " + e.Message;
                _logger.Error("Unable to set message text: " + e.Message, e);
            }   
            return result;
        }
        public virtual string addAttachment(File file)
        {  
            string result = "";
            try
            {   
                if (file.exists())
                {  
                    BodyPart messageBodyPart = new MimeBodyPart();
                    DataSource src = new FileDataSource(file);
                    messageBodyPart.DataHandler = new DataHandler(src);
                    messageBodyPart.FileName = file.Name;
                    this.multipart.addBodyPart(messageBodyPart);
                }
                else
                {
                    throw new Exception("File does not exist: " + file.AbsolutePath + "/" + file.Name);
                }
            }
            catch (Exception e)
            {  
                result = "Unable to add message attachment: " + e.Message;
                _logger.Error("Unable to add message attachment: " + e.Message);
            }   
            return result;
        }
        public virtual Session Session
        {
            get
            {    
                return this.session;
            }
            set
            {  
                this.session = value;
            }
        }
        public virtual MimeMessage Message
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
    }
}