namespace MenuOrder.Shared.Services.Interface
{
    public interface IMailService
    {
        bool SendMailWithSingleRecipient(string toAddress, string subject, string body);
    }
}
