using MenuManagment.Domain.Mongo.Entities;
using System.Threading.Tasks;

namespace MenuManagment.Domain.Mongo.Interfaces
{
    public interface IVendorCartRepository
    {
        Task<VendorCartDetails> GetVendorConfigurationByVendorId(string VendorId);
    }
}
