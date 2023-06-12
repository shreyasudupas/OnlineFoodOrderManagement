using Inventory.Microservice.Core.Common.Models.InventoryService.Request;
using Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Command;
using Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Query;
using Inventory.Microservice.Core.Services.MenuInventoryService.MenuImage.Query.Models;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Query;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Models;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    public class MenuImageController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public MenuImageController(IWebHostEnvironment environment,
            ILogger<MenuImageController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost("/api/menuimage/upload/manual")]
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
            var result = await Mediator.Send(new AddMenuImageCommand { MenuImageItem = model }); 

            return result;
        }

        [HttpGet("/api/menuimage/list")]
        public async Task<ImageResponse> GetAllMenuImages([FromQuery] int Skip,int Size)
        {
            var model = new ImageResponse();
            var result = await Mediator.Send(new GetAllMenuImagesQuery { Pagination = new Pagination { Skip = Skip , Limit = Size } });

            var count = await Mediator.Send(new GetMenuImagesRecordCountQuery());
            if (count > 0)
                model.TotalRecord = count;

            if(result == null)
            {
                return model;
            }

            var tasks = result?.Select(image => GetImageModel(image));

            var taskResult = await Task.WhenAll(tasks);
            
            foreach(var res in taskResult)
            {
                model.ImageData.Add(res);
            }

            return model;
        }

        public async  Task<ImageDataModel> GetImageModel(MenuImageDto menuImageDto)
        {
            if(menuImageDto != null)
            {
                byte[] bytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_environment.WebRootPath, "MenuImages", menuImageDto.FileName));

                return new ImageDataModel
                {
                    Id = menuImageDto.Id,
                    ItemName = menuImageDto.ItemName,
                    Active = menuImageDto.Active,
                    Data = Convert.ToBase64String(bytes, 0, bytes.Length),
                    Description = menuImageDto.Description
                };
            }
            else
            {
                return new ImageDataModel();
            }
            
        }

        [HttpGet("/api/menuimage/{menuImageId}")]
        public async Task<ImageDataModel> GetMenuImageById(string menuImageId)
        {
            var imageModelResult = await Mediator.Send(new GetMenuImageByIdQuery { Id = menuImageId });
            if(imageModelResult != null)
            {
                var result = await GetImageModel(imageModelResult);
                return result;
            }else
                return null;
        }

        [HttpPut("/api/menuimage/upload")]
        public async Task<MenuImageDto> UpdateMenuImage([FromBody] MenuImageUpload menuImageUpload)
        {
            if(!string.IsNullOrEmpty(menuImageUpload.Image.Data) && !string.IsNullOrEmpty(menuImageUpload.Image.Type))
            {
                var menuImage = await Mediator.Send(new GetMenuImageByIdQuery { Id = menuImageUpload.Id });

                if (menuImage != null)
                {
                    //check of file already exists
                    if (System.IO.File.Exists(menuImage.ImagePath))
                    {
                        System.IO.File.Delete(menuImage.ImagePath);
                    }
                    byte[] bytes = Convert.FromBase64String(menuImageUpload.Image.Data);
                    string fileName = menuImage.ItemName + "_" + menuImage.Id + '.' + menuImageUpload.Image.Type;

                    //set the image path
                    string imgPath = Path.Combine(_environment.WebRootPath, "MenuImages", fileName);

                    System.IO.File.WriteAllBytes(imgPath, bytes);

                    var update = await Mediator.Send(new UpdateMenuImagesCommand 
                    { 
                        UpdateMenuImage = new MenuImageDto
                        {
                            Id = menuImage.Id,
                            Active = menuImageUpload.Active,
                            Description = menuImageUpload.Description,
                            FileName = fileName, //updated filename
                            ImagePath = imgPath, //updated path
                            ItemName = menuImage.ItemName
                        }
                    });

                    return update;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var menuImage = await Mediator.Send(new GetMenuImageByIdQuery { Id = menuImageUpload.Id });

                //update other than image parameters
                var update = await Mediator.Send(new UpdateMenuImagesCommand
                {
                    UpdateMenuImage = new MenuImageDto
                    {
                        Id = menuImage.Id,
                        Active = menuImageUpload.Active,
                        Description = menuImageUpload.Description,
                        FileName = menuImage.FileName,
                        ImagePath = menuImage.ImagePath, 
                        ItemName = menuImageUpload.ItemName
                    }
                });

                return update;
            }
        }

        [HttpPost("/api/menuimage/upload")]
        public async Task<MenuImageDto> AddMenuImage([FromBody] MenuImageUpload menuImageUpload)
        {
            if(!string.IsNullOrEmpty(menuImageUpload.Image.Data) && !string.IsNullOrEmpty(menuImageUpload.Image.Type))
            {
                var result = await Mediator.Send(new AddMenuImageCommand
                {
                    MenuImageItem = new MenuImageDto
                    {
                        Id = menuImageUpload.Id,
                        ItemName = menuImageUpload.ItemName,
                        Active = menuImageUpload.Active,
                        Description = menuImageUpload.Description,
                        FileName = "",
                        ImagePath = ""
                    }
                });

                if (result != null)
                {
                    menuImageUpload.Id = result.Id;
                    byte[] bytes = Convert.FromBase64String(menuImageUpload.Image.Data);
                    string fileName = menuImageUpload.ItemName + "_" + menuImageUpload.Id + '.' + menuImageUpload.Image.Type;

                    //set the image path
                    string imgPath = Path.Combine(_environment.WebRootPath, "MenuImages", fileName);
                    System.IO.File.WriteAllBytes(imgPath, bytes);

                    var updateResult = await Mediator.Send(new UpdateMenuImagesCommand
                    {
                        UpdateMenuImage = new MenuImageDto
                        {
                            Id = menuImageUpload.Id,
                            Active = menuImageUpload.Active,
                            Description = menuImageUpload.Description,
                            FileName = fileName, //updated filename
                            ImagePath = imgPath, //updated path
                            ItemName = menuImageUpload.ItemName
                        }
                    });

                    if (updateResult != null)
                        return updateResult;
                    else
                    {
                        _logger.LogError($"AddMenuImage:Error updating Image path and fileName");
                        return null;
                    }
                }
                else
                {
                    _logger.LogError("Error in saving the menu Image");
                    return null;
                }
            }else
            {
                _logger.LogError("Base64 Data / Image type is missing from Image request");
                return null;
            }
        }

        [HttpGet("/api/menuimage/search")]
        public async Task<List<ImageDataModel>> GetMenuImagesByParam([FromQuery] string searchParam)
        {
            var response = new List<ImageDataModel>();
            var result = await Mediator.Send(new GetMenuImagesByItemName { SearchParam = searchParam });

            if(result != null)
            {
                foreach(var image in result)
                {
                    var getImageModel = await GetImageModel(image);

                    response.Add(getImageModel);
                }
            }

            return response;
        }

        [HttpGet("/api/menuimage/{menuImageId}/fileName")]
        public async Task<string> GetImageFileName(string menuImageId)
        {
            var imageResult = await Mediator.Send(new GetMenuImageByIdQuery { Id = menuImageId });
            if (imageResult != null)
            {
                return imageResult.FileName;
            }
            else
                return null;
            
        }

    }
}
