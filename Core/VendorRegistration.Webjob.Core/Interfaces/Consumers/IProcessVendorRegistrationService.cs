namespace MenuManagement.MessagingQueue.Core.Interfaces.Consumers
{
    public interface IProcessVendorRegistrationService
    {
        Task ProcessVendorRegistration(string vendorModelMessage);

        //void InitilizeMessage();
    }
}
