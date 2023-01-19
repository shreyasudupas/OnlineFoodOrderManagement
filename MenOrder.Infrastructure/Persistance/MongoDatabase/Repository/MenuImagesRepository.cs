using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Core.Mongo.Models;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Extension;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class MenuImagesRepository : BaseRepository<MenuImages> , IMenuImagesRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public MenuImagesRepository(IMongoDBContext context
            ,ILogger<MenuImagesRepository> logger
            ,IMapper mapper
            ) : base(context)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<MenuImageDto>> GetAllMenuImages(Pagination pagination)
        {
            var menusImages = await GetAllItemsByPagination(pagination);
            if(menusImages.ToList().Count > 0)
            {
                var mapToDto = _mapper.Map<List<MenuImageDto>>(menusImages);
                return mapToDto;
            }
            else
            {
                _logger.LogInformation("No Images exists");
                return null;
            }
        }

        public async Task<MenuImageDto> GetMenuImagesById(string Id)
        {
            var menusImages = await GetById(Id);
            if (menusImages != null)
            {
                var mapToDto = _mapper.Map<MenuImageDto>(menusImages);
                return mapToDto;
            }
            else
            {
                _logger.LogInformation($"No Images exists for Id: {Id}");
                return null;
            }
        }

        public async Task<MenuImageDto> AddMenuImage(MenuImageDto menuImageDto)
        {
            _logger.LogInformation("AddMenuImage started...");

            var mapToModel = _mapper.Map<MenuImages>(menuImageDto);
            await CreateOneDocument(mapToModel);

            var insertedMenuItem = await GetByFilter(i => i.FileName == menuImageDto.FileName);
            menuImageDto.Id = insertedMenuItem.Id;

            return menuImageDto;
        }

        public async Task<MenuImageDto> UpdateMenuImage(MenuImageDto menuImageDto)
        {
            _logger.LogInformation("UpdateMenuImage started...");
            var menuImage = await GetById(menuImageDto.Id);
            if(menuImage != null)
            {
                var mapToModel = _mapper.Map<MenuImages>(menuImageDto);
                var filter = Builders<MenuImages>.Filter.Eq(x => x.Id, menuImage.Id);
                var update = Builders<MenuImages>.Update.ApplyMultiFields(mapToModel);

                var result = await UpdateOneDocument(filter, update);

                if(result.IsAcknowledged)
                {
                    _logger.LogInformation("update success");
                    return menuImageDto;
                }
                else
                {
                    _logger.LogError("Error Updating the image form");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"UpdateMenuImage menuItem not found {menuImage.Id}");
                return null;
            }
        }

        public bool IfMenuImageDocumentExists()
        {
            var result = IfDocumentExists();
            return result == 0 ? false : true;
        }

        public async Task<int> GetMenuImageRecordCount()
        {
            var result = await GetAllItems();
            return result.ToList().Count;
        }
    }
}
