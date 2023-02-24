using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IVendorFoodTypeRepository
    {
        Task<List<VendorFoodType>> GetListVendorFoodType(bool active);
        Task<VendorFoodType> GetVendorFoodTypeById(string Id);
        Task<VendorFoodType> AddVendorFoodType(VendorFoodTypeDto FoodType);
        Task<VendorFoodType> UpdateFoodTypeDocument(VendorFoodTypeDto vendorFoodTypeDto);
        public int IfVendorFoodTypeCollectionExists();
    }
}
