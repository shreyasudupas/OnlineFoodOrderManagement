using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Extension;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
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

        public async Task<List<VendorFoodTypeDto>> GetListVendorFoodType(bool active)
        {
            _logger.LogInformation("GetListVendorType started");
            var foodTypes = await GetAllItems();
            var mapToDtoModel = _mapper.Map<List<VendorFoodTypeDto>>(foodTypes);
            if (active)
                mapToDtoModel = mapToDtoModel.FindAll(x => x.Active == true);

            return mapToDtoModel;
        }

        public async Task<VendorFoodTypeDto> GetVendorFoodTypeById(string Id)
        {
            _logger.LogInformation("GetVendorTypeById started");
            var foodTypeById = await GetById(Id);
            if(foodTypeById != null)
            {
                var mapToDto = _mapper.Map<VendorFoodTypeDto>(foodTypeById);
                return mapToDto;
            }else
            {
                _logger.LogError($"Food Type with Id: {Id} not available");
                return null;
            }
        }

        public async Task<VendorFoodTypeDto> AddVendorFoodType(VendorFoodTypeDto FoodType)
        {
            _logger.LogInformation("AddVendorFoodTypeById started");
            var ifExists = await GetByFilter(f => f.TypeName == FoodType.TypeName);
            if (ifExists == null)
            {
                var mapDtoToModel = _mapper.Map<VendorFoodType>(FoodType);
                await CreateOneDocument(mapDtoToModel);

                var updatedFoodType = await GetByFilter(f => f.TypeName == FoodType.TypeName);
                FoodType.Id = updatedFoodType.Id;
                return FoodType;
            }
            else
            {
                _logger.LogError("Food Type already exists");
                return FoodType;
            }
        }

        public async Task<VendorFoodTypeDto> UpdateFoodTypeDocument(VendorFoodTypeDto vendorFoodTypeDto)
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

                var mapToVendorFoodTypeModelToDto = _mapper.Map<VendorFoodTypeDto>(getVendorWithId);

                return mapToVendorFoodTypeModelToDto;
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
