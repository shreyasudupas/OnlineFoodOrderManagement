using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.GetAllCategories;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        [HttpGet("/api/category/all")]
        public async Task<List<CategoryDto>> GetAllCategory()
        {
            return await Mediator.Send(new GetAllCategoriesQuery());
        }
    }
}
