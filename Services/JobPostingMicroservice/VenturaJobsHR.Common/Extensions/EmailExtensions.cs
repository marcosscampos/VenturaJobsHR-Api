using System.Net;
using System.Net.Mail;

namespace VenturaJobsHR.Common.Extensions;

public class EmailExtensions
{
	public static void SendEmail(Email email, List<string> to, string body, string subject)
	{
		SmtpClient client = new SmtpClient(email.Host, email.Port);

		if (!string.IsNullOrEmpty(email.Login) && !string.IsNullOrEmpty(email.Password))
		{
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(email.Login, email.Password);
		}

		client.EnableSsl = email.SSL;

		MailMessage mailMessage = new()
        {
			IsBodyHtml = true,
			From = new MailAddress(email.Login)
		};

		foreach (var t in to)
		{
			mailMessage.To.Add(t);
		}

		mailMessage.Body = body;
		mailMessage.Subject = subject;
		client.Send(mailMessage);
	}
}

public class Email
{
	public string Host { get; set; }
	public int Port { get; set; }
	public bool SSL { get; set; }
	public string Login { get; set; }
	public string Password { get; set; }
}