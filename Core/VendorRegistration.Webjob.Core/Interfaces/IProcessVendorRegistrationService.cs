using VendorRegistration.Webjob.Core.Models;

namespace VendorRegistration.Webjob.Core.Interfaces
{
    public interface IProcessVendorRegistrationService
    {
        Task ProcessVendorRegistration(string vendorModelMessage);

        //void InitilizeMessage();
    }
}
