using MailKit.Net.Smtp;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using System.Net;

namespace Demo.PL.Utilities
{
    public class Email
    {
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Recipient { get; set; } = null!;
    }
    public class Setting()
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }

    }
    public interface IMailSetting
    {
        public Task SendEmailAsync(Email email);

	}
    public class MailSetting(IOptions<Setting> option):IMailSetting
    {
        public async Task SendEmailAsync(Email email)
        {
            var messege = new MimeMessage();
            messege.To.Add(MailboxAddress.Parse(email.Recipient));
            messege.From.Add(new MailboxAddress(option.Value.DisplayName , option.Value.Email));
            messege.Subject = email.Subject;
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            messege.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(option.Value.Host , option.Value.Port , MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(option.Value.Email, option.Value.Password);
            await smtp.SendAsync(messege);
            smtp.Disconnect(true);
		}
    }
}
