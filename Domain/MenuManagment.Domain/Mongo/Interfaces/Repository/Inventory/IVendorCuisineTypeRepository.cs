using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IVendorCuisineTypeRepository
    {
        Task<List<VendorCuisineType>> GetListVendorCuisineType(bool active);
        Task<VendorCuisineType> GetVendorCuisineTypeById(string Id);
        Task<VendorCuisineType> AddVendorCuisineType(VendorCuisineDto vendorCuisine);
        Task<VendorCuisineType> UpdateFoodTypeDocument(VendorCuisineDto vendorCuisine);
        int IfVendorCuisineDocumentExists();
    }
}
