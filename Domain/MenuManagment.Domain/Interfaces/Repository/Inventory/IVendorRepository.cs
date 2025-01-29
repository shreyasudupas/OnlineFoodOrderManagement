using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using MenuManagment.Mongo.Domain.Dtos.Inventory;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IVendorRepository
    {
        Task<Vendor> AddVendorDocument(Vendor vendor);
        Task<List<Vendor>> AddVendorDocuments(List<Vendor> vendors);
        Task<List<Vendor>> GetAllVendorDocuments();
        int IfVendorCollectionExists();

        Task<Vendor> GetVendorDocument(string id);
        Task<Vendor> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression);
        Task<VendorCategory> AddCategoryToVendor(string vendorId, VendorCategory vendorCategory);
        Task<List<VendorCategory>> GetAllVendorCategories(string vendorId);
        Task<Vendor> UpdateVendorDocument(Vendor vendorData);
        Task<VendorCategory> GetCategoryById(string Id, string VendorId);
        Task<VendorCategory> UpdateVendorCategoryDocument(string vendorId, VendorCategory categoryDto);

        Task<List<Vendor>> GetNearestDistanceOfVendorsByRadiusInKM(double latitude, double longitude, double distanceInKm);

        Task<bool> UpdateVendorStatus(string vendorId, string status);
        Task<VendorCategoryMenu> GetVendorMenuListWithCategoryIdAsync(string vendorId, string categoryId, CancellationToken cancellationToken);
    }
}
