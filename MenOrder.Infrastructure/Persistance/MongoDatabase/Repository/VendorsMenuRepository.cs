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
    public class VendorsMenuRepository : BaseRepository<VendorsMenus> , IVendorsMenuRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public VendorsMenuRepository(IMongoDBContext mongoDBContext,
            ILogger<VendorsMenuRepository> logger,
            IMapper mapper) : base(mongoDBContext)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VendorMenuDto> AddVendorMenus(VendorMenuDto menu)
        {
            _logger.LogInformation("AddVendorMenus started");

            if(menu != null)
            {
                var mapToMenuModel = _mapper.Map<VendorsMenus>(menu);

                mapToMenuModel.Items.ForEach(item => {
                    item.Id = ObjectId.GenerateNewId(System.DateTime.Now).ToString();
                });

                await CreateOneDocument(mapToMenuModel);

                var createdMenu = await GetByFilter(m => m.VendorId == menu.VendorId);

                if(createdMenu != null)
                {
                    var mapToDto = _mapper.Map<VendorMenuDto>(createdMenu);
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

        public async Task<List<VendorMenuDto>> GetAllMenu()
        {
            _logger.LogInformation("GetAllMenu started");

            var menus = await GetAllItems();

            if(menus.ToList().Count > 0)
            {
                var mapModel = _mapper.Map<List<VendorMenuDto>>(menus);

                _logger.LogInformation("GetAllMenu ended");
                return mapModel;
            }
            else
            {
                _logger.LogInformation("GetAllMenu Menu present");
                return new List<VendorMenuDto>();
            }
        }

        public async Task<List<MenuItemsDto>> GetAllVendorMenuItems(string VendorId)
        {
            _logger.LogInformation($"GetAllVendorMenuItems started with id: {VendorId}");

            var menu = await GetByFilter(v=>v.VendorId == VendorId);

            if (menu != null)
            { 
                var mapMenuItemDto = _mapper.Map<List<MenuItemsDto>>(menu.Items);

                _logger.LogInformation("GetAllVendorMenuItems ended");
                return mapMenuItemDto;
            }
            else
            {
                _logger.LogInformation($"GetAllVendorMenuItems Menu Items not present for id: {VendorId}");
                return new List<MenuItemsDto>();
            }
        }

        public async Task<MenuItemsDto> AddMenuItem(string vendorMenuId,MenuItemsDto menuItemsDto)
        {
            _logger.LogInformation($"AddMenuItem started with new menu item: {JsonConvert.SerializeObject(menuItemsDto)}");

            var menu = await GetById(vendorMenuId);
            if(menu != null)
            {
                var mapModelMenuItem = _mapper.Map<MenuItems>(menuItemsDto);

                mapModelMenuItem.Id = ObjectId.GenerateNewId(System.DateTime.Now).ToString();
                var filter = Builders<VendorsMenus>.Filter.Eq(x=>x.Id, vendorMenuId);
                var update = Builders<VendorsMenus>.Update.Push(m=>m.Items, mapModelMenuItem);

                var result = await UpdateOneDocument(filter,update);
                if(result.IsAcknowledged)
                {
                    var updateMenu = await GetById(vendorMenuId);
                    var updatedMenuItems = updateMenu.Items.Where(x => x.ItemName == menuItemsDto.ItemName).FirstOrDefault();
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
                _logger.LogInformation($"AddMenuItem menu Id:{vendorMenuId}");
                return null;
            }
        }
    }
}
