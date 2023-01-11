using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Extension;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public VendorRepository(IMongoDBContext mongoDBContext,
            ILogger<VendorRepository> logger,
            IMapper mapper
            ) : base(mongoDBContext)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VendorDto> AddVendorDocument(VendorDto vendor)
        {
            _logger.LogInformation("AddVendorDocument started..");
            if (vendor != null)
            {
                var mapToVendorModel = _mapper.Map<Vendor>(vendor);

                await CreateOneDocument(mapToVendorModel);

                var getVendorWithId = await GetByFilter(v=>v.VendorName == vendor.VendorName);

                var vendorWithIdMapTo = _mapper.Map<VendorDto>(getVendorWithId);

                return vendorWithIdMapTo;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<VendorDto>> AddVendorDocuments(List<VendorDto> vendors)
        {
            _logger.LogInformation("AddVendorDocuments started..");
            if(vendors.Count > 0)
            {
                var mapToVendorModel = _mapper.Map<List<Vendor>>(vendors);

                await CreateManyDocument(mapToVendorModel);

                return vendors;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<VendorDto>> GetAllVendorDocuments()
        {
            _logger.LogInformation("GetAllVendorDocuments started..");

            var vendors = await GetAllItems();

            if(vendors.ToList().Count > 0)
            {
                var mapToVendorDto = _mapper.Map<List<VendorDto>>(vendors);

                return mapToVendorDto;
            }
            else
            {
                _logger.LogInformation("No Vendors is database");
                return new List<VendorDto>();
            }
        }

        public async Task<VendorDto> GetVendorDocument(string id)
        {
            _logger.LogInformation("GetVendorDocument started..");

            var vendor = await GetById(id);

            if (vendor != null)
            {
                var mapToVendorDto = _mapper.Map<VendorDto>(vendor);

                return mapToVendorDto;
            }
            else
            {
                _logger.LogInformation($"Vendors with id: {id} is database");
                return new VendorDto();
            }
        }

        public async Task<VendorDto> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression)
        {
            _logger.LogInformation("GetVendorDocumentByCustomerfilter started..");

            return await GetVendorDocumentByCustomerfilter(filterExpression);
        }

        public int IfVendorCollectionExists()
        {
            return IfDocumentExists();
        }

        public async Task<CategoryDto> AddCategoryToVendor(string vendorId,CategoryDto category)
        {
            var vendor = await GetById(vendorId);

            if(vendor != null)
            {
                var mapDtoToCategory = _mapper.Map<Categories>(category);

                var vendorCategoryExists = vendor.Categories.Any(c => c.Name == category.Name);
                if(!vendorCategoryExists)
                {
                    //add id to categories
                    mapDtoToCategory.Id = ObjectId.GenerateNewId(DateTime.Now).ToString();

                    var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorId);
                    var update = Builders<Vendor>.Update.Push(m => m.Categories, mapDtoToCategory);

                    var result = await UpdateOneDocument(filter, update);

                    if (result.IsAcknowledged)
                    {
                        var vendorUpdated = await GetById(vendorId);
                        var updatedCategory = vendorUpdated.Categories.Find(x => x.Name == category.Name);
                        var mapToDto = _mapper.Map<CategoryDto>(updatedCategory);
                        return mapToDto;
                    }
                    else
                    {
                        _logger.LogError("Error saving thge category to vendor");
                        return null;
                    }
                }
                else
                {
                    _logger.LogError($"category with Name: {category.Name} already exists");
                    return null;
                }
            }
            else
            {
                _logger.LogError("Vendor Id not found");
                return null;
            }
        }

        public async Task<List<CategoryDto>> GetAllVendorCategories(string vendorId)
        {
            var vendor = await GetById(vendorId);

            if (vendor != null)
            {
                var mapToDtoCatgories = _mapper.Map<List<CategoryDto>>(vendor.Categories);
                return mapToDtoCatgories;
            }
            else
            {
                _logger.LogError($"Vendor with id {vendorId} not present ");
                return null;
            }
        }

        public async Task<CategoryDto> GetCategoryById(string Id,string VendorId)
        {
            _logger.LogInformation("GetCategoryById started..");
            var vendor = await GetById(VendorId);
            if(vendor != null)
            {
                var categoryList = vendor.Categories.Find(c => c.Id == Id);
                var mapModelToDto = _mapper.Map<CategoryDto>(categoryList);
                return mapModelToDto;
            }
            else
            {
                _logger.LogError($"Vendor with id:{VendorId} not present");
                return null;
            }
        }

        public async Task<VendorDto> UpdateVendorDocument(VendorDto vendorData)
        {
            _logger.LogInformation("UpdateVendorDocument started..");
            var vendor = await GetById(vendorData.Id);

            if (vendor != null)
            {
                var mapToVendorModel = _mapper.Map<Vendor>(vendorData);
                //update category since FE is not sending category
                mapToVendorModel.Categories = vendor.Categories;

                var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorData.Id);
                var update = Builders<Vendor>.Update.ApplyMultiFields(mapToVendorModel);

                var result = await UpdateOneDocument(filter, update);
                if(result.IsAcknowledged)
                {
                    var getVendorWithId = await GetByFilter(v => v.Id == vendor.Id);

                    var vendorWithIdMapTo = _mapper.Map<VendorDto>(getVendorWithId);

                    return vendorWithIdMapTo;
                }
                else
                {
                    _logger.LogError($"Error updating the Vendor with Id {vendorData.Id}");
                    return vendorData;
                }
            }
            else
            {
                _logger.LogError("No Items present to update");
                return null;
            }
        }

        public async Task<CategoryDto> UpdateVendorCategoryDocument(string vendorId,CategoryDto categoryDto)
        {
            _logger.LogInformation("UpdateVendorCategoryDocument started..");
            var vendor = await GetById(vendorId);

            if (vendor != null)
            {
                var mapToCategoryModel = _mapper.Map<Categories>(categoryDto);

                //update category since FE is not sending category
                var itemToBeUpdated = vendor.Categories.Where(x => x.Id == mapToCategoryModel.Id).FirstOrDefault();


                var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorId)
                    & Builders<Vendor>.Filter.ElemMatch(v=>v.Categories,f=>f.Id == mapToCategoryModel.Id);
                var update = Builders<Vendor>.Update.Set(f=>f.Categories[-1],mapToCategoryModel);

                var result = await UpdateOneDocument(filter, update);
                if (result.IsAcknowledged)
                {
                    var mapToCategoryDtoModel = _mapper.Map<CategoryDto>(mapToCategoryModel);
                    return mapToCategoryDtoModel;
                }
                else
                {
                    _logger.LogError($"Error updating the Vendor with Id {vendor}");
                    return categoryDto;
                }
            }
            else
            {
                _logger.LogError("No Items present to update");
                return null;
            }
        }
    }
}
