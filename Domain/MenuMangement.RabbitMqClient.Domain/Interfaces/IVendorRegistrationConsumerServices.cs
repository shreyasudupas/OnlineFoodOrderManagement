namespace MenuMangement.RabbitMqClient.Domain.Interfaces
{
    public interface IVendorRegistrationConsumerServices
    {
        void GetVendorRegistrationMessageFromQueue();
    }
}
