using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Command;
using MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Commands;
using MenuManagement.Core.Services.MenuInventoryService.VendorMenus.Query;
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

        [HttpGet("/api/vendorMenus/list/{vendorId}")]
        public async Task<List<VendorMenuDto>> GetAllVendorMenuItemsByVendorId(string vendorId)
        {
            return await Mediator.Send(new GetAllVendorMenuItemsQuery { VendorId = vendorId });
        }

        [HttpPost("/api/vendormenus")]
        public async Task<VendorMenuDto> AddVendorMenuItem(VendorMenuDto menu)
        {
            return await Mediator.Send(new AddVendorMenusCommand { Menu = menu });
        }

        [HttpGet("/api/vendormenus/{menuId}")]
        public async Task<VendorMenuDto> GetVendorMenuItemByMenuId(string menuId)
        {
            return await Mediator.Send(new GetVendorMenuItemByMenuIdQuery { MenuId = menuId });
        }

        [HttpPut("/api/vendormenus")]
        public async Task<VendorMenuDto> UpdateVendorMenuItem(UpdateVendorMenuCommand updateVendorMenu)
        {
            return await Mediator.Send(updateVendorMenu);
        }

        [HttpDelete("/api/vendormenus/{menuId}")]
        public async Task<bool> DeleteVendorMenuItem(string menuId)
        {
            return await Mediator.Send(new DeleteVendorMenuCommand { MenuId =  menuId});
        }
    }
}
