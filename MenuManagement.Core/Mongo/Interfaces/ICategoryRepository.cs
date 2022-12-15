using MenuManagement.Core.Common.Models.InventoryService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(string Id);
        Task<CategoryDto> AddCatgeory(CategoryDto category);
    }
}
