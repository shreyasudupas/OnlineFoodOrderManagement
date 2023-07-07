using Inventory.Microservice.Core.Common.Models.InventoryService.Response;
using Inventory.Microservice.Core.Services.VendorMenus.Commands;
using Inventory.Microservice.Core.Services.VendorMenus.Query;
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
using MenuManagment.Mongo.Domain.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json;

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
                if(!string.IsNullOrEmpty(result.ImageFilename) && !string.IsNullOrEmpty(result.ImageId))
                {
                    //Image Data is ImageFile Name
                    byte[] bytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_webHostEnvironment.WebRootPath, "MenuImages", result.ImageFilename));
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

        [AllowAnonymous]
        [HttpPost("/api/vendormenus/list")]
        public async Task<bool> AddListOfVendorMenu([FromForm] AddListVendorMenuModel addListVendorMenuModel)
        {
            try
            {
                var success = false;
                if (addListVendorMenuModel.UploadFile != null)
                {
                    using var reader = new StreamReader(addListVendorMenuModel.UploadFile.OpenReadStream());
                    var content = reader.ReadToEnd();

                    JSchemaGenerator generator = new JSchemaGenerator();
                    JSchema schema = generator.Generate(typeof(List<AddVendorMenuJson>));

                    JsonTextReader jsonReader = new JsonTextReader(new StringReader(content));

                    JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(jsonReader);
                    validatingReader.Schema = schema;

                    JsonSerializer serializer = new JsonSerializer();
                    var lists = serializer.Deserialize<List<AddVendorMenuJson>>(validatingReader);

                    if(lists.Count > 0)
                    {
                        success = await Mediator.Send(new AddVendorMenuJsonListCommand
                        {
                            JsonVendorMenuList = lists,
                            VendorId = addListVendorMenuModel.VendorId
                        });
                    }
                }
                else
                {
                    throw new Exception("UploadFile Parameter is null");
                }
                

                return success;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
