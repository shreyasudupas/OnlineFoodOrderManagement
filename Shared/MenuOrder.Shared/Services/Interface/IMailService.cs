namespace MenuOrder.Shared.Services.Interface
{
    public interface IMailService
    {
        void SendMailWithSingleRecipient(string toAddress, string body);
    }
}
