using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IVendorUserMappingService
    {
        Task<List<VendorUserIdMapping>> GetVendorUserMapping(string VendorId);
        Task<VendorUserIdMapping> AddVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping);
        Task<VendorUserIdMapping> UpdateVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping);
    }
}
