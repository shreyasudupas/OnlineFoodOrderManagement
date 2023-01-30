using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDb.Shared.Persistance.Repositories;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Extensions;

namespace Inventory.Mongo.Persistance.Persistance.MongoDatabase.Repository
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

        public async Task<List<MenuImages>> GetAllMenuImages(Pagination pagination)
        {
            var menusImages = await GetAllItemsByPagination(pagination);
            if(menusImages.ToList().Count > 0)
            {
                return menusImages.ToList();
            }
            else
            {
                _logger.LogInformation("No Images exists");
                return null;
            }
        }

        public async Task<MenuImages> GetMenuImagesById(string Id)
        {
            var menusImages = await GetById(Id);
            if (menusImages != null)
            {
                return menusImages;
            }
            else
            {
                _logger.LogInformation($"No Images exists for Id: {Id}");
                return null;
            }
        }

        public async Task<MenuImages> AddMenuImage(MenuImageDto menuImageDto)
        {
            _logger.LogInformation("AddMenuImage started...");

            var mapToModel = _mapper.Map<MenuImages>(menuImageDto);
            await CreateOneDocument(mapToModel);

            var insertedMenuItem = await GetByFilter(i => i.FileName == menuImageDto.FileName);
            menuImageDto.Id = insertedMenuItem.Id;

            return insertedMenuItem;
        }

        public async Task<MenuImages> UpdateMenuImage(MenuImageDto menuImageDto)
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
                    return mapToModel;
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

        public async Task<List<MenuImages>> GetImagesBySearchParam(string searchParam)
        {
            var queryExpr = new BsonRegularExpression(new Regex(searchParam, RegexOptions.IgnoreCase));

            var filter = Builders<MenuImages>.Filter.Regex(f => f.ItemName, queryExpr);
            var result = await GetAllMatchItems(filter);
            if(result.Count > 0)
            {
                return result;
            }
            else
            {
                _logger.LogInformation("No Items got");
                return null;
            }
        }
    }
}
