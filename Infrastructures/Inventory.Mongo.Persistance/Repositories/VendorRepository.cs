using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDb.Shared.Persistance.Repositories;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Extensions;
using MongoDB.Libmongocrypt;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Inventory.Mongo.Persistance.Repositories
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

        public async Task<Vendor> AddVendorDocument(VendorDto vendor)
        {
            _logger.LogInformation("AddVendorDocument started..");

            await AssignIndexForVendor();

            if (vendor != null)
            {
                var mapToVendorModel = _mapper.Map<Vendor>(vendor);

                await CreateOneDocument(mapToVendorModel);

                var getVendorWithId = await GetByFilter(v => v.VendorName == vendor.VendorName);

                return getVendorWithId;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<Vendor>> AddVendorDocuments(List<VendorDto> vendors)
        {
            _logger.LogInformation("AddVendorDocuments started..");

            await AssignIndexForVendor();

            if (vendors.Count > 0)
            {
                var mapToVendorModel = _mapper.Map<List<Vendor>>(vendors);

                await CreateManyDocument(mapToVendorModel);

                return mapToVendorModel;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<Vendor>> GetAllVendorDocuments()
        {
            _logger.LogInformation("GetAllVendorDocuments started..");

            await AssignIndexForVendor();

            var vendors = await GetAllItems();

            if (vendors.ToList().Count > 0)
            {
                return vendors.ToList();
            }
            else
            {
                _logger.LogInformation("No Vendors is database");
                return new List<Vendor>();
            }
        }

        public async Task<Vendor> GetVendorDocument(string id)
        {
            _logger.LogInformation("GetVendorDocument started..");

            var vendor = await GetById(id);

            if (vendor != null)
            {
                return vendor;
            }
            else
            {
                _logger.LogInformation($"Vendors with id: {id} is database");
                return null;
            }
        }

        public async Task<Vendor> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression)
        {
            _logger.LogInformation("GetVendorDocumentByCustomerfilter started..");

            return await GetVendorDocumentByCustomerfilter(filterExpression);
        }

        public int IfVendorCollectionExists()
        {
            return IfDocumentExists();
        }

        public async Task<Categories> AddCategoryToVendor(string vendorId, CategoryDto category)
        {
            var vendor = await GetById(vendorId);

            if (vendor != null)
            {
                var mapDtoToCategory = _mapper.Map<Categories>(category);

                var vendorCategoryExists = vendor.Categories.Any(c => c.Name == category.Name);
                if (!vendorCategoryExists)
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
                        return updatedCategory;
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

        public async Task<List<Categories>> GetAllVendorCategories(string vendorId)
        {
            var vendor = await GetById(vendorId);

            if (vendor != null)
            {
                return vendor.Categories;
            }
            else
            {
                _logger.LogError($"Vendor with id {vendorId} not present ");
                return null;
            }
        }

        public async Task<Categories> GetCategoryById(string Id, string VendorId)
        {
            _logger.LogInformation("GetCategoryById started..");
            var vendor = await GetById(VendorId);
            if (vendor != null)
            {
                var categoryList = vendor.Categories.Find(c => c.Id == Id);
                return categoryList;
            }
            else
            {
                _logger.LogError($"Vendor with id:{VendorId} not present");
                return null;
            }
        }

        public async Task<Vendor> UpdateVendorDocument(VendorDto vendorData)
        {
            try
            {
                _logger.LogInformation("UpdateVendorDocument started..");
                var vendor = await GetById(vendorData.Id);

                var mapToVendorModel = _mapper.Map<Vendor>(vendorData);
                if (vendor != null)
                {

                    //update category since FE is not sending category and Registrion Process as well
                    mapToVendorModel.Categories = vendor.Categories;
                    mapToVendorModel.RegistrationProcess = vendor.RegistrationProcess;

                    var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorData.Id);
                    var update = Builders<Vendor>.Update.ApplyMultiFields(mapToVendorModel);

                    var result = await UpdateOneDocument(filter, update);
                    if (result.IsAcknowledged)
                    {
                        var getVendorWithId = await GetByFilter(v => v.Id == vendor.Id);
                        return getVendorWithId;
                    }
                    else
                    {
                        _logger.LogError($"Error updating the Vendor with Id {vendorData.Id}");
                        return mapToVendorModel;
                    }
                }
                else
                {
                    _logger.LogError("No Items present to update");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return null;
            }

        }

        public async Task<Categories> UpdateVendorCategoryDocument(string vendorId, CategoryDto categoryDto)
        {
            _logger.LogInformation("UpdateVendorCategoryDocument started..");
            var mapToCategoryModel = _mapper.Map<Categories>(categoryDto);
            var vendor = await GetById(vendorId);

            if (vendor != null)
            {
                //update category since FE is not sending category
                var itemToBeUpdated = vendor.Categories.Where(x => x.Id == mapToCategoryModel.Id).FirstOrDefault();


                var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorId)
                    & Builders<Vendor>.Filter.ElemMatch(v => v.Categories, f => f.Id == mapToCategoryModel.Id);
                var update = Builders<Vendor>.Update.Set(f => f.Categories[-1], mapToCategoryModel);

                var result = await UpdateOneDocument(filter, update);
                if (result.IsAcknowledged)
                {
                    return mapToCategoryModel;
                }
                else
                {
                    _logger.LogError($"Error updating the Vendor with Id {vendor}");
                    return mapToCategoryModel;
                }
            }
            else
            {
                _logger.LogError("No Items present to update");
                return null;
            }
        }

        public async Task AssignIndexForVendor()
        {
            var indexKeyDefinitionBuilder = Builders<Vendor>.IndexKeys;

            var vendorIdIndexModel = new CreateIndexModel<Vendor>
                (indexKeyDefinitionBuilder.Ascending(indexKey => indexKey.VendorName),new CreateIndexOptions
                {
                    Unique = true,
                });

            var coordinatesIndexModel = new CreateIndexModel<Vendor>
                (indexKeyDefinitionBuilder.Geo2DSphere(indexKey => indexKey.Coordinates));

            var indexModels = new List<CreateIndexModel<Vendor>>
            {
                vendorIdIndexModel , coordinatesIndexModel
            };

            await CreateMultipleIndexAsync(indexModels);
        }

        // Create geospatial query that searches for restaurants that fall within a radius (in KM).
        //refer https://www.mongodb.com/developer/languages/csharp/mongodb-geospatial-queries-csharp/
        public async Task<List<Vendor>> GetNearestDistanceOfVendorsByRadiusInKM(double latitude,double longitude,double distanceInKm)
        {
            var builder = Builders<Vendor>.Filter;
            var point = GeoJson.Point(GeoJson.Position(latitude, longitude));

            var filter = builder.GeoWithinCenterSphere(v => v.Coordinates, point.Coordinates.X,point.Coordinates.Y, distanceInKm / 6378.1); //distance(kilometers) to radians

            var result = await GetListByFilterDefinition(filter);

            return result;
        }

        public async Task<bool> UpdateVendorStatus(string vendorId,string status)
        {
            var vendor = await GetById(vendorId);

            if(vendor != null)
            {
                var filter = Builders<Vendor>.Filter.Eq(x => x.Id, vendorId);
                var update = Builders<Vendor>.Update.Set(m => m.RegistrationProcess, status);

                var result = await UpdateOneDocument(filter, update);

                if(result.IsAcknowledged)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } 
            else
            {
                return false;
            }

        }
    }
}
