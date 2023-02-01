using Inventory.Microservice.Core.Common.Models.InventoryService.Response;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Command;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Commands;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Query;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class VendorMenuController : BaseController
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VendorMenuController(ILogger<VendorMenuController> logger, 
            IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

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
        public async Task<VendorMenuResponse> GetVendorMenuItemByMenuId(string menuId)
        {
            var result = await Mediator.Send(new GetVendorMenuItemByMenuIdQuery { MenuId = menuId });

            if(result != null)
            {
                if(!string.IsNullOrEmpty(result.ImageData) && !string.IsNullOrEmpty(result.ImageId))
                {
                    //Image Data is ImageFile Name
                    byte[] bytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_webHostEnvironment.WebRootPath, "MenuImages", result.ImageData));
                    var imagebase64 = Convert.ToBase64String(bytes, 0, bytes.Length);
                    result.ImageData = imagebase64;
                }
                return result;
            }
            else
            {
                logger.LogError($"Error occucred retriving Vendor menu Detail with id:{menuId}");
                return null;
            }
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
