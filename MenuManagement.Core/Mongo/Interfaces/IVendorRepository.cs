using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorRepository
    {
        Task<List<VendorDto>> AddVendorDocuments(List<VendorDto> vendors);
        Task<List<VendorDto>> GetAllVendorDocuments();
        int IfVendorCollectionExists();
    }
}
