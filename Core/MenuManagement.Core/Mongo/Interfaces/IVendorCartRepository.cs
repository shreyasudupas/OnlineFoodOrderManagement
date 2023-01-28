using MenuManagement.Core.Mongo.Entities;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorCartRepository
    {
        Task<VendorCartDetails> GetVendorConfigurationByVendorId(string VendorId);
    }
}
