using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository
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
