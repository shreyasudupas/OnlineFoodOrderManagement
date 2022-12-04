using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using Microsoft.Extensions.Logging;
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
    }
}
