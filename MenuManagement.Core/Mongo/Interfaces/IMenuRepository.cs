using MenuManagement.Core.Common.Models.InventoryService;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IMenuRepository
    {
        Task<MenuDto> AddMenu(MenuDto menu)
    }
}
