using Company.pro.PL.Settings;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Company.pro.PL.Helper
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {


        public bool SendEmail(Email email)
        {
            try
            {
                // Build Message
                var mail = new MimeMessage();

                mail.Subject = email.Subject;
                mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
                mail.To.Add(MailboxAddress.Parse(email.To));

                var builder = new BodyBuilder();
                builder.TextBody = email.Body;
                mail.Body = builder.ToMessageBody();

                // Establish connection

                using var smpt = new SmtpClient();
                smpt.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smpt.Authenticate(_options.Value.Email, _options.Value.Password);

                // Send Message
                smpt.Send(mail);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

       
    }
}
  