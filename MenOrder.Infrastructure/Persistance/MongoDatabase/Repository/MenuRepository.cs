using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class MenuRepository : BaseRepository<Menus> , IMenuRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public MenuRepository(IMongoDBContext mongoDBContext,
            ILogger<MenuRepository> logger,
            IMapper mapper) : base(mongoDBContext)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<MenuDto> AddMenu(MenuDto menu)
        {
            _logger.LogInformation("AddMenu started");

            if(menu != null)
            {
                var mapToMenuModel = _mapper.Map<Menus>(menu);

                mapToMenuModel.Items.ForEach(item => {
                    item.Id = ObjectId.GenerateNewId(System.DateTime.Now).ToString();
                });

                await CreateOneDocument(mapToMenuModel);

                var createdMenu = await GetByFilter(m => m.VendorId == menu.VendorId);

                if(createdMenu != null)
                {
                    var mapToDto = _mapper.Map<MenuDto>(createdMenu);
                    return mapToDto;
                }
                else
                {
                    _logger.LogInformation($"Menu with vendorId {menu.VendorId} not found");
                    return menu;
                }
            }
            else
            {
                _logger.LogError("No Items present");
                return null;
            }
        }

        public async Task<List<MenuDto>> GetAllMenu()
        {
            _logger.LogInformation("GetAllMenu started");

            var menus = await GetAllItems();

            if(menus.ToList().Count > 0)
            {
                var mapModel = _mapper.Map<List<MenuDto>>(menus);

                _logger.LogInformation("GetAllMenu ended");
                return mapModel;
            }
            else
            {
                _logger.LogInformation("GetAllMenu Menu present");
                return new List<MenuDto>();
            }
        }

        public async Task<List<MenuItemsDto>> GetAllMenuItem(string id)
        {
            _logger.LogInformation($"GetAllMenu started with id: {id}");

            var menu = await GetById(id);

            if (menu != null)
            { 
                var mapMenuItemDto = _mapper.Map<List<MenuItemsDto>>(menu.Items);

                _logger.LogInformation("GetAllMenuItem ended");
                return mapMenuItemDto;
            }
            else
            {
                _logger.LogInformation($"GetAllMenuItem Menu Items not present for id: {id}");
                return new List<MenuItemsDto>();
            }
        }

        public async Task<MenuItemsDto> AddMenuItem(string menuId,MenuItemsDto menuItemsDto)
        {
            _logger.LogInformation($"AddMenuItem started with new menu item: {JsonConvert.SerializeObject(menuItemsDto)}");

            var menu = await GetById(menuId);
            if(menu != null)
            {
                var mapModelMenuItem = _mapper.Map<MenuItems>(menuItemsDto);

                mapModelMenuItem.Id = ObjectId.GenerateNewId(System.DateTime.Now).ToString();
                var filter = Builders<Menus>.Filter.Eq(x=>x.Id, menuId);
                var update = Builders<Menus>.Update.Push(m=>m.Items, mapModelMenuItem);

                var result = await UpdateOneDocument(filter,update);
                if(result.IsAcknowledged)
                {
                    var updateMenu = await GetById(menuId);
                    var updatedMenuItems = updateMenu.Items.Where(x => x.Name == menuItemsDto.Name).FirstOrDefault();
                    var mapToUpdatedMenuItem = _mapper.Map<MenuItemsDto>(updatedMenuItems);
                    return mapToUpdatedMenuItem;
                }
                else
                {
                    _logger.LogError($"Update error");
                    return menuItemsDto;
                }
            }
            else
            {
                _logger.LogInformation($"AddMenuItem menu Id:{menuId}");
                return null;
            }
        }
    }
}
