using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDb.Shared.Persistance.Repositories;
using MongoDb.Shared.Persistance.Extensions;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Options;

namespace Inventory.Mongo.Persistance.Repositories
{
    public class VendorCuisineTypeRepository : BaseRepository<VendorCuisineType>, IVendorCuisineTypeRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public VendorCuisineTypeRepository(IMapper mapper
            , ILogger<VendorCuisineTypeRepository> logger,
            IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings
            ) : base(mongoDatabaseSettings)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VendorCuisineType>> GetListVendorCuisineType(bool active)
        {
            _logger.LogInformation("GetListVendorCuisineType started");
            var foodTypes = await GetAllItems();
            var foodTypeList = foodTypes.ToList();

            if (active)
                foodTypeList = foodTypeList.FindAll(x => x.Active == true);

            return foodTypeList;
        }

        public async Task<VendorCuisineType> GetVendorCuisineTypeById(string Id)
        {
            _logger.LogInformation("GetVendorCuisineTypeById started");
            var foodTypeById = await GetById(Id);
            if (foodTypeById != null)
            {
                return foodTypeById;
            }
            else
            {
                _logger.LogError($"Cuisine Type with Id: {Id} not available");
                return null;
            }
        }

        public async Task<VendorCuisineType> AddVendorCuisineType(VendorCuisineDto vendorCuisine)
        {
            _logger.LogInformation("AddVendorCuisineTypeById started");
            var ifExists = await GetDocumentByFilter(f => f.CuisineName == vendorCuisine.CuisineName);
            if (ifExists == null)
            {
                var mapDtoToModel = _mapper.Map<VendorCuisineType>(vendorCuisine);
                await CreateOneDocument(mapDtoToModel);

                var updatedFoodType = await GetDocumentByFilter(f => f.CuisineName == vendorCuisine.CuisineName);
                return updatedFoodType;
            }
            else
            {
                _logger.LogError("Food Cuisine already exists");
                var mapToDto = _mapper.Map<VendorCuisineType>(vendorCuisine);
                return mapToDto;
            }
        }

        public async Task<VendorCuisineType> UpdateFoodTypeDocument(VendorCuisineDto vendorCuisine)
        {
            _logger.LogInformation("UpdateFoodTypeDocument started..");
            var vendor = await GetById(vendorCuisine.Id);

            if (vendor != null)
            {
                var mapToVendorFoodTypeModel = _mapper.Map<VendorCuisineType>(vendorCuisine);

                var filter = Builders<VendorCuisineType>.Filter.Eq(x => x.Id, vendorCuisine.Id);
                var update = Builders<VendorCuisineType>.Update.ApplyMultiFields(mapToVendorFoodTypeModel);

                var result = await UpdateOneDocument(filter, update);

                var getVendorWithId = await GetDocumentByFilter(v => v.Id == vendor.Id);

                return getVendorWithId;
            }
            else
            {
                _logger.LogError("No Items present to update");
                return null;
            }
        }

        public int IfVendorCuisineDocumentExists()
        {
            return IfDocumentExists();
        }
    }
}
