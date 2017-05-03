namespace EmailSender
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Net.Mail;
	using System.Reflection;
	using System.Threading;

	public interface IEmailSender: IHideObjectMembers, IDisposable
	{		
		MailMessage Message { get; set; }
		IEmailSender UsingClient(SmtpClient client);
		IEmailSender To(string emailAddress, string name);
		IEmailSender To(string emailAddress);
		IEmailSender To(IList<MailAddress> mailAddresses);
		IEmailSender CC(string emailAddress, string name = "");
		IEmailSender CC(IList<MailAddress> mailAddresses);
		IEmailSender BCC(string emailAddress, string name = "");
		IEmailSender BCC(IList<MailAddress> mailAddresses);
		IEmailSender ReplyTo(string address);
		IEmailSender ReplyTo(string address, string name);
		IEmailSender Subject(string subject);
		IEmailSender Body(string body);
		IEmailSender HighPriority();
		IEmailSender LowPriority();
		IEmailSender Attach(Attachment attachment);
		IEmailSender Attach(IList<Attachment> attachments);
		IEmailSender BodyAsHtml();
		IEmailSender BodyAsPlainText();
		IEmailSender UsingTemplateEngine(ITemplateRenderer renderer);
		IEmailSender UsingTemplateFromEmbedded<T>(string path, T model, Assembly assembly = null);
		IEmailSender UsingTemplateFromFile<T>(string filename, T model);
		IEmailSender UsingCultureTemplateFromFile<T>(string filename, T model, CultureInfo culture = null);
		IEmailSender UsingTemplate<T>(string template, T model, bool isHtml = true);		
		IEmailSender Send();
		IEmailSender SendAsync(SendCompletedEventHandler callback, object token = null);
		IEmailSender Cancel();
		
	}
}
