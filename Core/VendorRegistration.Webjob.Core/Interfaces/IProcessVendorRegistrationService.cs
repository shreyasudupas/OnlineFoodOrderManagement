using MenuManagement.Webjob.Core.Models;

namespace MenuManagement.Webjob.Core.Interfaces
{
    public interface IProcessVendorRegistrationService
    {
        Task ProcessVendorRegistration(string vendorModelMessage);

        //void InitilizeMessage();
    }
}
