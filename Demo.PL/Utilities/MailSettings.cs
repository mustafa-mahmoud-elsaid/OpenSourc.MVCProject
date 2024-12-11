using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utilities
{
    public class Email
    {
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Recipient { get; set; } = null!;
    }
    public static class MailSettings
    {
        public static void SendEmail(Email email)
        {
            
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            string sender = "mostafapro87@gmail.com";
            client.Credentials = new NetworkCredential(sender, "axkywwsvstpeofcy");
            client.Send(sender, email.Recipient, email.Subject, email.Body);
        }
    }
}
