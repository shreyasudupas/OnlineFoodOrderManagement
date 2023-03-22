using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MenuOrder.Shared.Services.Interface;

namespace MenuOrder.Shared.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMailWithSingleRecipient(string toAddress,string body)
        {
            var mailMessage = new MimeMessage();
            var fromAddress = _configuration.GetSection("MailSettings:FromAddress").Value;
            var fromName = _configuration.GetSection("MailSettings:FromName").Value;
            var toName = toAddress.Split('@')[0];
            var smtpHost = _configuration.GetSection("MailSettings:SmptHost").Value;
            var username = _configuration.GetSection("MailSettings:Username").Value;
            var password = _configuration.GetSection("MailSettings:Password").Value;


            mailMessage.To.Add(new MailboxAddress(toName, toAddress));
            mailMessage.From.Add(new MailboxAddress(fromName, fromAddress));
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(smtpHost, 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate(username, password);

                smtp.Send(mailMessage);
                smtp.Disconnect(true);
            }
        }
    }
}
