using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class VendorMenuController : BaseController
    {
        [HttpGet("/api/vendorMenus")]
        public async Task<List<VendorMenuDto>> GetAllVendorMenus()
        {
            return await Mediator.Send(new GetAllVendorMenusQuery());
        }

        [HttpGet("/api/vendorMenus/menuItems/list/{vendorId}")]
        public async Task<List<MenuItemsDto>> GetAllVendorMenuItemsByVendorId(string vendorId)
        {
            return await Mediator.Send(new GetAllVendorMenuItemsQuery { VendorId = vendorId });
        }

        [HttpPost("/api/vendormenus")]
        public async Task<VendorMenuDto> AddVendorMenuItem(VendorMenuDto menu)
        {
            return await Mediator.Send(new AddVendorMenusCommand { Menu = menu });
        }

        [HttpPost("/api/vendormenus/menuItems/add")]
        public async Task<MenuItemsDto> AddMenuItem([FromBody]AddMenuItemCommand addMenuItemCommand)
        {
            return await Mediator.Send(addMenuItemCommand);
        }
    }
}
