using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IMenuRepository
    {
        Task<MenuDto> AddMenu(MenuDto menu);
        Task<List<MenuDto>> GetAllMenu();

        Task<List<MenuItemsDto>> GetAllMenuItem(string id);

        Task<MenuItemsDto> AddMenuItem(string menuId, MenuItemsDto menuItemsDto);
    }
}
