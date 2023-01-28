using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Core.Mongo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IMenuImagesRepository
    {
        Task<List<MenuImageDto>> GetAllMenuImages(Pagination pagination);
        Task<MenuImageDto> GetMenuImagesById(string Id);
        Task<MenuImageDto> AddMenuImage(MenuImageDto menuImageDto);
        Task<MenuImageDto> UpdateMenuImage(MenuImageDto menuImageDto);
        bool IfMenuImageDocumentExists();
        Task<int> GetMenuImageRecordCount();

        Task<List<MenuImageDto>> GetImagesBySearchParam(string searchParam);
    }
}
