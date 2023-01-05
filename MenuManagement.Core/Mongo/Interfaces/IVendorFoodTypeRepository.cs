using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorFoodTypeRepository
    {
        Task<List<VendorFoodTypeDto>> GetListVendorFoodType(bool active);
        Task<VendorFoodTypeDto> GetVendorFoodTypeById(string Id);
        Task<VendorFoodTypeDto> AddVendorFoodType(VendorFoodTypeDto FoodType);
        Task<VendorFoodTypeDto> UpdateFoodTypeDocument(VendorFoodTypeDto vendorFoodTypeDto);
        public int IfVendorFoodTypeCollectionExists();
    }
}
