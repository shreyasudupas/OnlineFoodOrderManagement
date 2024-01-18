using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IVendorUserMappingService
    {
        Task<List<VendorMappingResponse>> GetVendorUserMapping(string VendorId);
        Task<VendorUserIdMapping> AddVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping);
        Task<VendorUserIdMapping> UpdateVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping, bool enabled);
        Task<VendorUserIdMapping> GetVendorUserMappingBasedOnEmailId(string emailId);
        Task<VendorUserIdMapping> GetVendorUserMapping(string userId, string vendorId);
    }
}
