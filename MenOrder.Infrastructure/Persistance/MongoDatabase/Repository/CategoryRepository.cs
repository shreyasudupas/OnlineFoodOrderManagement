using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class CategoryRepository : BaseRepository<Categories>, ICategoryRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CategoryRepository(
            IMongoDBContext mongoDBContext,
            ILogger<CategoryRepository> logger,
            IMapper mapper) : base(mongoDBContext)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await GetAllItems();

            if(categories.ToList().Count > 0)
            {
                var mapToDto = _mapper.Map<List<CategoryDto>>(categories);
                return mapToDto;
            }
            else
            {
                _logger.LogInformation("GetAllCategories is empty in database");
                return null;
            }
        }

        public async Task<CategoryDto> GetCategoryById(string Id)
        {
            _logger.LogInformation($"GetCategoryById with category: {Id}");

            var category = await GetById(Id);
            if(category != null)
            {
                var mapToDto = _mapper.Map<CategoryDto>(category);
                return mapToDto;
            }else
            {
                _logger.LogError($"GetCategoryById with category: {Id} not found");
                return null;
            }
        }

        public async Task<CategoryDto> AddCatgeory(CategoryDto category)
        {
            _logger.LogInformation($"AddCatgeory with category: {JsonConvert.SerializeObject(category)}");
            var mapToCategory = _mapper.Map<Categories>(category);

            await CreateOneDocument(mapToCategory);

            var newCategory = await GetByFilter(c=>c.Name == category.Name);

            _logger.LogInformation($"AddCatgeory with category after adding: {JsonConvert.SerializeObject(newCategory)}");
            var mapToCategoryDto = _mapper.Map<CategoryDto>(newCategory);

            return mapToCategoryDto;
        }
    }
}
