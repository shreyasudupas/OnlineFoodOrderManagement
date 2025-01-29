using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.Extensions.Logging;
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
        public VendorsMenuRepository(
            IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings,
            ILogger<VendorsMenuRepository> logger) : base(mongoDatabaseSettings)
        {
            _logger = logger;
        }

        public async Task<VendorsMenus> AddVendorMenus(VendorsMenus vendorMenus)
        {
            _logger.LogInformation("AddVendorMenus started");

            var ifExists = await GetDocumentByFilter(vm => vm.ItemName == vendorMenus.ItemName && vm.VendorId == vendorMenus.VendorId);
            if (ifExists == null)
            {
                await CreateOneDocument(vendorMenus);

                var createdMenu = await GetDocumentByFilter(m => m.VendorId == vendorMenus.VendorId && m.ItemName == vendorMenus.ItemName);

                if (createdMenu != null)
                {
                    return createdMenu;
                }
                else
                {
                    _logger.LogInformation($"Menu with vendorId {vendorMenus.VendorId} not found");
                    return vendorMenus;
                }
            }
            else
            {
                _logger.LogError("ItemName already present");
                return vendorMenus;
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

        public async Task<VendorsMenus> UpdateVendorMenus(VendorsMenus vendorsMenus)
        {
            _logger.LogInformation("UpdateVendorMenus started..");
            var vendorMenu = await GetById(vendorsMenus.Id);
            if (vendorMenu != null)
            {
                
                var filter = Builders<VendorsMenus>.Filter.Eq(vm => vm.Id, vendorsMenus.Id);
                var update = Builders<VendorsMenus>.Update.ApplyMultiFields(vendorsMenus);

                var result = await UpdateOneDocument(filter, update);
                if (result.IsAcknowledged)
                {
                    _logger.LogInformation("UpdateVendorMenus update complete");
                    return vendorsMenus;
                }
                else
                {
                    _logger.LogError($"Error updating Vendor Menus");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"No vendor menu with Id: {vendorsMenus.Id}");
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

        public async Task<bool> AddVendorMenuList(List<VendorsMenus> vendorsMenus)
        {
            try
            {
                _logger.LogInformation("Add List VendorMenu started...");

                await CreateManyDocument(vendorsMenus);

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
