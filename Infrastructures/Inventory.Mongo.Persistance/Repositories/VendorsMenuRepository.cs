using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Extensions;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Options;

namespace Inventory.Mongo.Persistance.Repositories
{
    public class VendorsMenuRepository : BaseRepository<VendorsMenus>, IVendorsMenuRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public VendorsMenuRepository(
            IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings,
            ILogger<VendorsMenuRepository> logger,
            IMapper mapper) : base(mongoDatabaseSettings)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VendorsMenus> AddVendorMenus(VendorMenuDto menu)
        {
            _logger.LogInformation("AddVendorMenus started");
            var mapToMenuModel = _mapper.Map<VendorsMenus>(menu);

            if (menu != null)
            {
                var ifExists = await GetDocumentByFilter(vm => vm.ItemName == menu.ItemName && vm.VendorId == menu.VendorId);
                if (ifExists == null)
                {
                    await CreateOneDocument(mapToMenuModel);

                    var createdMenu = await GetDocumentByFilter(m => m.VendorId == menu.VendorId && m.ItemName == menu.ItemName);

                    if (createdMenu != null)
                    {
                        return createdMenu;
                    }
                    else
                    {
                        _logger.LogInformation($"Menu with vendorId {menu.VendorId} not found");
                        return mapToMenuModel;
                    }
                }
                else
                {
                    _logger.LogError("ItemName already present");
                    return mapToMenuModel;
                }
            }
            else
            {
                _logger.LogError("No Items present");
                return null;
            }
        }

        public async Task<List<VendorsMenus>> GetAllMenu()
        {
            _logger.LogInformation("GetAllMenu started");

            var menus = await GetAllItems();

            if (menus.ToList().Count > 0)
            {
                _logger.LogInformation("GetAllMenu ended");
                return menus.ToList();
            }
            else
            {
                _logger.LogInformation("GetAllMenu Menu present");
                return new List<VendorsMenus>();
            }
        }

        public async Task<List<VendorsMenus>> GetAllVendorMenuByVendorId(string VendorId)
        {
            _logger.LogInformation($"GetAllVendorMenuByVendorId started with id: {VendorId}");

            var menu = await ListDocumentsByFilter(v => v.VendorId == VendorId);

            if (menu != null)
            {
                _logger.LogInformation("GetAllVendorMenuByVendorId ended");
                return menu;
            }
            else
            {
                _logger.LogInformation($"GetAllVendorMenuItems Menu Items not present for id: {VendorId}");
                return null;
            }
        }

        public async Task<VendorsMenus> GetVendorMenusByMenuId(string menuId)
        {
            _logger.LogInformation("GetVendorMenusByMenuId started..");
            var menuItem = await GetById(menuId);
            if (menuItem != null)
            {
                _logger.LogInformation("GetVendorMenusByMenuId ended..");
                return menuItem;
            }
            else
            {
                _logger.LogError($"Menu Item for MenuId: {menuId}");
                return null;
            }
        }

        public async Task<VendorsMenus> UpdateVendorMenus(VendorMenuDto menu)
        {
            _logger.LogInformation("UpdateVendorMenus started..");
            var vendorMenu = await GetById(menu.Id);
            if (vendorMenu != null)
            {
                var mapToVendorMenusModel = _mapper.Map<VendorsMenus>(menu);
                var filter = Builders<VendorsMenus>.Filter.Eq(vm => vm.Id, mapToVendorMenusModel.Id);
                var update = Builders<VendorsMenus>.Update.ApplyMultiFields(mapToVendorMenusModel);

                var result = await UpdateOneDocument(filter, update);
                if (result.IsAcknowledged)
                {
                    _logger.LogInformation("UpdateVendorMenus update complete");
                    return mapToVendorMenusModel;
                }
                else
                {
                    _logger.LogError($"Error updating Vendor Menus");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"No vendor menu with Id: {menu.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteVendorMenu(string menuId)
        {
            _logger.LogInformation("DeleteVendorMenu started...");
            var menu = await GetById(menuId);
            if (menu != null)
            {
                var filter = Builders<VendorsMenus>.Filter.Eq(vm => vm.Id, menuId);
                var result = await DeleteOneDocument(filter);
                if (result.IsAcknowledged)
                {
                    return true;
                }
                else
                {
                    _logger.LogError($"Error in Deleting Menu with Id: {menuId}");
                    return false;
                }
            }
            else
            {
                _logger.LogError($"DeleteVendorMenu {menuId} not present in the database");
                return false;
            }
        }

        public async Task<bool> AddVendorMenuList(List<VendorMenuDto> vendorsMenuDtos)
        {
            try
            {
                _logger.LogInformation("Add List VendorMenu started...");

                var mapToVendorMenus = _mapper.Map<List<VendorsMenus>>(vendorsMenuDtos);

                await CreateManyDocument(mapToVendorMenus);

                _logger.LogInformation("Add List VendorMenu ended...");

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Inserting Vendor Menu list encountred with error {ex.Message}");
                return false;
            }
        }
    }
}
