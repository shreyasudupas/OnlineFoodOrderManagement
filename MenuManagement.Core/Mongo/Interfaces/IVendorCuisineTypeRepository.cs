using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorCuisineTypeRepository
    {
        Task<List<VendorCuisineDto>> GetListVendorCuisineType(bool active);
        Task<VendorCuisineDto> GetVendorCuisineTypeById(string Id);
        Task<VendorCuisineDto> AddVendorCuisineType(VendorCuisineDto vendorCuisine);
        Task<VendorCuisineDto> UpdateFoodTypeDocument(VendorCuisineDto vendorCuisine);
        int IfVendorCuisineDocumentExists();
    }
}
