using MenuOrder.Shared.Enum;

namespace MenuOrder.Shared.Services.Interface
{
    public interface IMailService
    {
        bool SendMailWithSingleRecipient(string toAddress, string subject, string body);
        string SendEmailTemplateBody(EmailTypeEnum emailType, params string[] param);
    }
}
