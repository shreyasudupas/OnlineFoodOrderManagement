using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IVendorRepository
    {
        Task<Vendor> AddVendorDocument(VendorDto vendor);
        Task<List<Vendor>> AddVendorDocuments(List<VendorDto> vendors);
        Task<List<Vendor>> GetAllVendorDocuments();
        int IfVendorCollectionExists();

        Task<Vendor> GetVendorDocument(string id);
        Task<Vendor> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression);
        Task<Categories> AddCategoryToVendor(string vendorId, CategoryDto category);
        Task<List<Categories>> GetAllVendorCategories(string vendorId);
        Task<Vendor> UpdateVendorDocument(VendorDto vendorData);
        Task<Categories> GetCategoryById(string Id, string VendorId);
        Task<Categories> UpdateVendorCategoryDocument(string vendorId, CategoryDto categoryDto);

        Task<List<Vendor>> GetNearestDistanceOfVendorsByRadiusInKM(double latitude, double longitude, double distanceInKm);

        Task<bool> UpdateVendorStatus(string vendorId, string status);
    }
}
