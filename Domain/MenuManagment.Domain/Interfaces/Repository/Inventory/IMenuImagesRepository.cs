using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository
{
    public interface IMenuImagesRepository
    {
        Task<List<MenuImages>> GetAllMenuImages(Pagination pagination);
        Task<MenuImages> GetMenuImagesById(string Id);
        Task<MenuImages> AddMenuImage(MenuImageDto menuImageDto);
        Task<MenuImages> UpdateMenuImage(MenuImageDto menuImageDto);
        bool IfMenuImageDocumentExists();
        Task<int> GetMenuImageRecordCount();

        Task<List<MenuImages>> GetImagesBySearchParam(string searchParam);
        Task<MenuImages> DeleteImageById(string id);
    }
}
