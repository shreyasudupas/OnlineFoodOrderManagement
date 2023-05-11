using MenuManagement.MessagingQueue.Core.Models;

namespace MenuManagement.MessagingQueue.Core.Interfaces.Producers
{
    public interface IVendorRegistrationProducerService
    {
        void PublishVendorInformationToQueue(VendorModel vendorModel);
    }
}
