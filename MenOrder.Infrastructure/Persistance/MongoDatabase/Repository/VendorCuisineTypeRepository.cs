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
    public class VendorCuisineTypeRepository : BaseRepository<VendorCuisineType> , IVendorCuisineTypeRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public VendorCuisineTypeRepository(IMapper mapper
            ,ILogger<VendorCuisineTypeRepository> logger,
            IMongoDBContext mongoDBContext) : base(mongoDBContext)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VendorCuisineDto>> GetListVendorCuisineType(bool active)
        {
            _logger.LogInformation("GetListVendorCuisineType started");
            var foodTypes = await GetAllItems();
            var mapToDtoModel = _mapper.Map<List<VendorCuisineDto>>(foodTypes);
            if (active)
                mapToDtoModel = mapToDtoModel.FindAll(x => x.Active == true);

            return mapToDtoModel;
        }

        public async Task<VendorCuisineDto> GetVendorCuisineTypeById(string Id)
        {
            _logger.LogInformation("GetVendorCuisineTypeById started");
            var foodTypeById = await GetById(Id);
            if (foodTypeById != null)
            {
                var mapToDto = _mapper.Map<VendorCuisineDto>(foodTypeById);
                return mapToDto;
            }
            else
            {
                _logger.LogError($"Cuisine Type with Id: {Id} not available");
                return null;
            }
        }

        public async Task<VendorCuisineDto> AddVendorCuisineType(VendorCuisineDto vendorCuisine)
        {
            _logger.LogInformation("AddVendorCuisineTypeById started");
            var ifExists = await GetByFilter(f => f.CuisineName == vendorCuisine.CuisineName);
            if (ifExists == null)
            {
                var mapDtoToModel = _mapper.Map<VendorCuisineType>(vendorCuisine);
                await CreateOneDocument(mapDtoToModel);

                var updatedFoodType = await GetByFilter(f => f.CuisineName == vendorCuisine.CuisineName);
                vendorCuisine.Id = updatedFoodType.Id;
                return vendorCuisine;
            }
            else
            {
                _logger.LogError("Food Cuisine already exists");
                return vendorCuisine;
            }
        }

        public async Task<VendorCuisineDto> UpdateFoodTypeDocument(VendorCuisineDto vendorCuisine)
        {
            _logger.LogInformation("UpdateFoodTypeDocument started..");
            var vendor = await GetById(vendorCuisine.Id);

            if (vendor != null)
            {
                var mapToVendorFoodTypeModel = _mapper.Map<VendorCuisineType>(vendorCuisine);

                var filter = Builders<VendorCuisineType>.Filter.Eq(x => x.Id, vendorCuisine.Id);
                var update = Builders<VendorCuisineType>.Update.ApplyMultiFields(mapToVendorFoodTypeModel);

                var result = await UpdateOneDocument(filter, update);

                var getVendorWithId = await GetByFilter(v => v.Id == vendor.Id);

                var mapToVendorCuisineTypeModelToDto = _mapper.Map<VendorCuisineDto>(getVendorWithId);

                return mapToVendorCuisineTypeModelToDto;
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
