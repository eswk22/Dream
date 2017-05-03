using System;
using System.Collections;
using System.Collections.Generic;
namespace ExecutionEngine.Common.Connect
{
    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    using StringUtils = Application.Utility.StringUtils;
    //using Email = org.apache.commons.mail.Email;
    //using EmailAttachment = org.apache.commons.mail.EmailAttachment;
    //using EmailException = org.apache.commons.mail.EmailException;
    //using HtmlEmail = org.apache.commons.mail.HtmlEmail;
    //using SimpleEmail = org.apache.commons.mail.SimpleEmail;
    using Application.Utility.Logging;
    using System.IO;
    using System.Net.Mail;
    using System.Net;

    public class EmailConnect2 : SessionObjectInterface
    {
        private ILogger _logger = new CrucialLogger();
        private readonly MailMessage message;
        private SmtpClient _client;
        public EmailConnect2(string host, int port, string username, string password, bool isSSL, bool hasAttachment) //, IDictionary<string, string> properties) 
            : this(host, port, username, password, isSSL, true, hasAttachment)//, properties)
        {
        }
        public EmailConnect2(string host, int port, string username, string password, bool isSSL, bool isHtml, bool hasAttachment)//, IDictionary<string, string> properties)
        {
            _client = new SmtpClient();
            _client.Host = host;
            _client.Port = port;
            _client.EnableSsl = isSSL;
            if ((hasAttachment) || (isHtml))
            {
                this.message.IsBodyHtml = true;
            }
            else
            {
                this.message.IsBodyHtml = false;
            }

            if ((StringUtils.isNotBlank(username)) && (StringUtils.isNotBlank(password)))
            {
                _client.Credentials = new NetworkCredential(username, password);
            }
            //if ((properties != null) && (properties.Count > 0))
            //{  
            //    _client
            //    this.message.MailSession.Properties.putAll(properties);
            //    _logger.Trace("Mail session properties:");
            //    foreach (string key in properties.Keys)
            //    {  
            //        _logger.Trace(key + ": " + this.message.MailSession.Properties.get(key));
            //    }
            //}
        }
        public virtual void close()
        {
        }
        ~EmailConnect2()
        {
        }
        public virtual string send()
        {
            string result = "";
            try
            {
                _logger.Trace("Mail Properties");
                //IDictionary properties = this.message.MailSession.Properties;
                //foreach (object key in properties.Keys)
                //{
                //    _logger.Trace("key: " + key + " value: " + properties[key]);
                //}
                _logger.Trace("From: " + this.message.From);
                _logger.Trace("To: " + this.message.To);
                _logger.Trace("Subject: " + this.message.Subject);
                _client.Send(this.message);
                result = "Send message Successfully";

            }
            catch (Exception e)
            {
                result = "Unable to send message : " + e.Message;
                _logger.Error("Unable to send message: " + e.Message, e);
            }
            return result;
        }
        public virtual string setFrom(string from, string fromDisplayName)
        {
            string result = "";
            try
            {
                if (StringUtils.isNotBlank(fromDisplayName))
                {
                    this.message.From = new MailAddress(from, fromDisplayName);
                }
                else
                {
                    this.message.From = new MailAddress(from);
                }
            }
            catch (Exception e)
            {
                result = "Unable to add From : " + e.Message;
                _logger.Error("Unable to add From : " + e.Message);
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
                    if (replyTo.IndexOf(',') > 0)
                    {
                        ICollection<string> addresses = StringUtils.stringToList(replyTo, ",");
                        foreach (string address in addresses)
                        {
                            this.message.ReplyToList.Add(address);
                        }
                    }
                    else
                        this.message.ReplyTo = new MailAddress(replyTo);
                }
                catch (Exception e)
                {
                    result = "Unable to add ReplyTo : " + e.Message;
                    _logger.Error(e.Message, e);
                }
            }
            return result;
        }
        public virtual string addTo(string addr)
        {
            string result = "";
            try
            {
                this.message.To.Add(addr);
            }
            catch (Exception e)
            {
                result = "Unable to add recipient: " + addr + " due to " + e.Message;
                _logger.Error("Unable to add recipient: " + addr + " due to " + e.Message);
            }
            return result;
        }
        public virtual string addTo(params string[] addr)
        {
            string result = "";
            try
            {
                foreach (string address in addr)
                    this.message.To.Add(address);
            }
            catch (Exception e)
            {
                result = "Unable to add recipient(s): " + addr + " due to " + e.Message;
                _logger.Error("Unable to add recipient(s): " + addr + " due to " + e.Message);
            }
            return result;
        }
        public virtual string addCC(string addr)
        {
            string result = "";
            try
            {
                this.message.CC.Add(addr);
            }
            catch (Exception e)
            {
                result = "Unable to add CC receipient: " + e.Message;
                /* 322 */
                _logger.Error("Unable to add CC receipient: " + e.Message);
            }   /* 324 */
            return result;
        }
        public virtual string addBCC(string addr)
        {   /* 336 */
            string result = "";
            try
            {  
                this.message.Bcc.Add(addr);
            }
            catch (Exception e)
            {   
                result = "Unable to add BCC receipient: " + e.Message;
               
                _logger.Error("Unable to BCC receipient: " + e.Message);
            }   /* 347 */
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
        public virtual string setHtml(string html)
        {
            string result = "";
            try
            {
                this.message.IsBodyHtml = true;
                this.message.Body = html;
            }
            catch (Exception e)
            {
                result = "ERROR: could not set html: " + e.Message;
                _logger.Error(result, e);
            }
            return result;
        }
        public virtual string setText(string text)
        {
            string result = "";
            try
            {
                this.message.IsBodyHtml = false;
                this.message.Body = text;
            }
            catch (Exception e)
            {
                result = "ERROR: could not set text: " + e.Message;
                _logger.Error(result, e);
            }
            return result;
        }
        public virtual string addAttachment(FileInfo file)
        {
            string result = "";
            if (file != null)
            {
                try
                {
                    if (file.Exists)
                    {
                        Attachment attachment = new Attachment(file.FullName);
                        attachment.Name = file.Name;
                        attachment.ContentDisposition.DispositionType = "attachment";
                        attachment.Name = file.Name;
                        if (this.message.IsBodyHtml)
                        {
                            this.message.Attachments.Add(attachment);
                        }
                        else
                        {
                            throw new Exception("Doesn't support attachment. Instantiate a different EmailConnect2 object. Check EmailConnect2 Javadoc for more information.");
                        }
                    }
                    else
                    {
                        throw new Exception("File does not exist: " + file.FullName);
                    }
                }
                catch (Exception e)
                {
                    result = "Unable to add message attachment: " + e.Message;
                    _logger.Error("Unable to add message attachment: " + e.Message);
                }
            }
            return result;
        }
    }
}