using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDb.Shared.Persistance.Repositories;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Extensions;

namespace MongoDb.Infrastructure.Persistance.Persistance.MongoDatabase.Repository
{
    public class VendorFoodTypeRepository : BaseRepository<VendorFoodType> , IVendorFoodTypeRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public VendorFoodTypeRepository(IMapper mapper,
            ILogger<VendorFoodTypeRepository> logger,
            IMongoDBContext mongoDBContext) : base(mongoDBContext)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VendorFoodType>> GetListVendorFoodType(bool active)
        {
            _logger.LogInformation("GetListVendorType started");
            var foodTypes = await GetAllItems();
            var foodTypesList = foodTypes.ToList();

            if (active)
                foodTypesList = foodTypesList.FindAll(x => x.Active == true);

            return foodTypesList;
        }

        public async Task<VendorFoodType> GetVendorFoodTypeById(string Id)
        {
            _logger.LogInformation("GetVendorTypeById started");
            var foodTypeById = await GetById(Id);
            if(foodTypeById != null)
            {
                return foodTypeById;
            }else
            {
                _logger.LogError($"Food Type with Id: {Id} not available");
                return null;
            }
        }

        public async Task<VendorFoodType> AddVendorFoodType(VendorFoodTypeDto FoodType)
        {
            _logger.LogInformation("AddVendorFoodTypeById started");
            var ifExists = await GetByFilter(f => f.TypeName == FoodType.TypeName);
            if (ifExists == null)
            {
                var mapDtoToModel = _mapper.Map<VendorFoodType>(FoodType);
                await CreateOneDocument(mapDtoToModel);

                var updatedFoodType = await GetByFilter(f => f.TypeName == FoodType.TypeName);
                return updatedFoodType;
            }
            else
            {
                _logger.LogError("Food Type already exists");
                var mapModel = _mapper.Map<VendorFoodType>(FoodType);
                return mapModel;
            }
        }

        public async Task<VendorFoodType> UpdateFoodTypeDocument(VendorFoodTypeDto vendorFoodTypeDto)
        {
            _logger.LogInformation("UpdateFoodTypeDocument started..");
            var vendor = await GetById(vendorFoodTypeDto.Id);

            if (vendor != null)
            {
                var mapToVendorFoodTypeModel = _mapper.Map<VendorFoodType>(vendorFoodTypeDto);

                var filter = Builders<VendorFoodType>.Filter.Eq(x => x.Id, vendorFoodTypeDto.Id);
                var update = Builders<VendorFoodType>.Update.ApplyMultiFields(mapToVendorFoodTypeModel);

                var result = await UpdateOneDocument(filter, update);

                var getVendorWithId = await GetByFilter(v => v.Id == vendor.Id);

                return getVendorWithId;
            }
            else
            {
                _logger.LogError("No Items present to update");
                return null;
            }
        }

        public int IfVendorFoodTypeCollectionExists()
        {
            return IfDocumentExists();
        }
    }
}
