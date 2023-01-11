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
                var ifExists = await GetByFilter(vm => vm.ItemName == menu.ItemName && vm.VendorId == menu.VendorId);
                if(ifExists == null)
                {
                    var mapToMenuModel = _mapper.Map<VendorsMenus>(menu);

                    await CreateOneDocument(mapToMenuModel);

                    var createdMenu = await GetByFilter(m => m.VendorId == menu.VendorId && m.ItemName == menu.ItemName);

                    if (createdMenu != null)
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
                    _logger.LogError("ItemName already present");
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

        public async Task<List<VendorMenuDto>> GetAllVendorMenuByVendorId(string VendorId)
        {
            _logger.LogInformation($"GetAllVendorMenuByVendorId started with id: {VendorId}");

            var menu = await GetListByFilter(v=>v.VendorId == VendorId);

            if (menu != null)
            { 
                var mapMenuItemDto = _mapper.Map<List<VendorMenuDto>>(menu);

                _logger.LogInformation("GetAllVendorMenuByVendorId ended");
                return mapMenuItemDto;
            }
            else
            {
                _logger.LogInformation($"GetAllVendorMenuItems Menu Items not present for id: {VendorId}");
                return null;
            }
        }
    }
}
