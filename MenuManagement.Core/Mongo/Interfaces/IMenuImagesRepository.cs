using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IMenuImagesRepository
    {
        Task<List<MenuImageDto>> GetAllMenuImages();
        Task<MenuImageDto> GetMenuImagesById(string Id);
        Task<MenuImageDto> AddMenuImage(MenuImageDto menuImageDto);
        Task<MenuImageDto> UpdateMenuImage(MenuImageDto menuImageDto);
        bool IfMenuImageDocumentExists();
    }
}
