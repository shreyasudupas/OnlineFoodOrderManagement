using MailKit.Net.Smtp;
using MimeKit;
using MenuOrder.Shared.Services.Interface;
using System;
using Microsoft.Extensions.Configuration;
using MenuOrder.Shared.Enum;
using MenuOrder.Shared.EmailTemplate;

namespace MenuOrder.Shared.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendMailWithSingleRecipient(string toAddress,string subject,string body)
        {
            var success = false;
            var mailMessage = new MimeMessage();
            var fromAddress = _configuration.GetSection("MailSettings:FromAddress").Value;
            //var fromName = _configuration.GetSection("MailSettings:FromName").Value;
            //var toName = toAddress.Split('@')[0];
            var smtpHost = _configuration.GetSection("MailSettings:SmptHost").Value;
            var username = _configuration.GetSection("MailSettings:Username").Value;
            var password = _configuration.GetSection("MailSettings:Password").Value;
            var port = Convert.ToInt32(_configuration.GetSection("MailSettings:Port").Value);


            mailMessage.To.Add(MailboxAddress.Parse(toAddress));
            mailMessage.From.Add(MailboxAddress.Parse(fromAddress));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(smtpHost, port, true);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate(username, password);

                smtp.Send(mailMessage);
                smtp.Disconnect(true);
                success = true;
            }

            return success;
        }

        public string SendEmailTemplateBody(EmailTypeEnum emailType, params string[] param)
        {
            string body = string.Empty;

            switch(emailType)
            {
                case EmailTypeEnum.RegisterVendor:
                    body = RegisterVendorTemplate.RegisterVendorEmail(param[0], param[1], param[2], param[3]);
                        break;
                case EmailTypeEnum.WelcomeVendor:
                    body = WelcomeVendorTemplate.WelocmeVendorEmail(param[0], param[1], param[2]);
                    break;

            }
            return body;
        }
    }
}
