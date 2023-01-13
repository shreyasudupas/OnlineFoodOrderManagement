using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Common.Models.InventoryService.Request;
using MenuManagement.Core.Services.MenuInventoryService.MenuImage.Command;
using MenuManagement.Core.Services.MenuInventoryService.MenuImage.Query;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    public class MenuImageController : BaseController
    {
        private readonly IWebHostEnvironment _environment;

        public MenuImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("/api/menuimage/upload")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<MenuImageDto> UploadImage([FromForm] MenuFormImageRequest menuFormImageRequest)
        {
            string path = "";
            string fileName = "";

            if (menuFormImageRequest.Image != null)
            {
                var file = menuFormImageRequest.Image;
                fileName = menuFormImageRequest.ItemName + "_" + Guid.NewGuid().ToString();
                path = Path.Combine(_environment.WebRootPath, "MenuImages", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var model = new MenuImageDto
            {
                ImagePath = path,
                FileName = fileName,
                Description = menuFormImageRequest.Description,
                Active = menuFormImageRequest.Active,
                ItemName = menuFormImageRequest.ItemName
            };
            var result = await Mediator.Send(new UploadImageCommand { MenuImageItem = model }); 

            return result;
        }

        [HttpGet("/api/menuimage/list")]
        public async Task<List<ImageResponse>> GetAllMenuImages()
        {
            var model = new List<ImageResponse>();
            string path = _environment.ContentRootPath;

            var result = await Mediator.Send(new GetAllMenuImagesQuery());

            result.ForEach(async image =>
            {
                var imagePath = image.ImagePath.Split("MenuImages")[1];
                var file = Directory.GetFiles(path);

                byte[] bytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_environment.WebRootPath, "MenuImages", image.FileName));

                model.Add(new ImageResponse
                {
                    ItemName = image.ItemName,
                    Active= image.Active,
                    Data = Convert.ToBase64String(bytes, 0, bytes.Length),
                    Description = image.Description
                });

            });

            return model;
        }
    }
}
