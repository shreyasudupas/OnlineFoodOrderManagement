using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.AddMenu;
using MenuManagement.Core.Services.MenuInventoryService.AddMenuItem;
using MenuManagement.Core.Services.MenuInventoryService.GetMenuItemDetail;
using MenuManagement.Core.Services.MenuInventoryService.MenuDetails;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        [HttpGet("/api/menus")]
        public async Task<List<MenuDto>> GetAllMenu()
        {
            return await Mediator.Send(new MenuQuery());
        }

        [HttpGet("/api/menu/{id}")]
        public async Task<List<MenuItemsDto>> GetMenuId(string id)
        {
            return await Mediator.Send(new GetMenuItemQuery { Id = id });
        }

        [HttpPost("/api/menus")]
        public async Task<MenuDto> AddMenu(MenuDto menu)
        {
            return await Mediator.Send(new AddMenuCommand { Menu = menu });
        }

        [HttpPost("/api/menus/menuItems")]
        public async Task<MenuItemsDto> AddMenuItem([FromBody]AddMenuItemCommand addMenuItemCommand)
        {
            return await Mediator.Send(addMenuItemCommand);
        }
    }
}
