using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace MenuManagement.InventoryMicroService.API
{
    public static class MigrateDummyDataInMongo
    {
        public static IHost UseMigration(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {

                var vendorService = scope.ServiceProvider.GetRequiredService<IVendorRepository>();
                var menuService = scope.ServiceProvider.GetRequiredService<IVendorsMenuRepository>();
                var foodTypeService = scope.ServiceProvider.GetRequiredService<IVendorFoodTypeRepository>();
                var cuisineTypeService = scope.ServiceProvider.GetRequiredService<IVendorCuisineTypeRepository>();
                var loggerContext = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var menuImageService = scope.ServiceProvider.GetRequiredService<IMenuImagesRepository>();
                var webHostService = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var menus = new List<VendorsMenus>();

                var dummyFoodType = new VendorFoodTypeDto
                {
                    Id = "",
                    TypeName = "Vegiterian",
                    Active = true
                };

                var dummyCuisineType = new VendorCuisineDto
                {
                    Id = "",
                    CuisineName = "Indian",
                    Active = true
                };

                var dummyVendors = new VendorDto
                {
                    Id = "",
                    AddressLine1 = "sample",
                    AddressLine2 = null,
                    Area = "Kathreguppe",
                    Categories = new List<CategoryDto>(),
                    City = "Bengaluru",
                    Coordinates = new CoordinatesDto
                    {
                        Latitude = 12.2334,
                        Longitute = 92.1212
                    },
                    State = "Karnataka",
                    Rating = 4,
                    CuisineType = new List<string> { "Indian" },
                    VendorDescription = "sample",
                    VendorName = "Vendor 1",
                    OpenTime =  "07:30:00",
                    CloseTime = "22:30:00",
                    Active= false
                };

                var dummyCategories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        Id = "",
                        Name="Breakfast",
                        Active=true,
                        Description="",
                        OpenTime = "07:30:00",
                        CloseTime = "11:30:00"
                    },
                    new CategoryDto
                    {
                        Id = "",
                        Name="Lunch",
                        Active=true,
                        Description="",
                        OpenTime = "11:30:00",
                        CloseTime = "15:00:00"
                    },
                    new CategoryDto
                    {
                        Id = "",
                        Name="Dinner",
                        Active=true,
                        Description="",
                        OpenTime="18:00:00",
                        CloseTime = "22:30:00"
                    },
                };

                var dummyMenu = new List<VendorMenuDto>
                {
                    new VendorMenuDto
                    {
                        Id= "",
                        VendorId = "",
                        ItemName = "Idly",
                        ImageId ="",
                        Price=20,
                        FoodType= dummyFoodType.TypeName,
                        Category = dummyCategories[0].Name,
                        Active=true,
                        Discount = 0
                    },
                    new VendorMenuDto
                    {
                        Id= "",
                        VendorId = "",
                        ItemName = "Plain Dosa",
                        ImageId ="",
                        Price=35,
                        FoodType= dummyFoodType.TypeName,
                        Category = dummyCategories[0].Name,
                        Active=true,
                        Discount = 10
                    },
                    new VendorMenuDto
                    {
                        Id= "",
                        VendorId="",
                        ItemName = "Mini Lunch",
                        ImageId ="",
                        Price=45,
                        FoodType= dummyFoodType.TypeName,
                        Category = dummyCategories[1].Name,
                        Active=true,
                        Discount = 0,
                        Rating = 3
                    }
                };

                var isExists = vendorService.IfVendorCollectionExists();

                if (isExists == 0)
                {
                    loggerContext.LogInformation("Migration Mongo Started..");

                    var vendor = vendorService.AddVendorDocument(dummyVendors).GetAwaiter().GetResult();

                    dummyCategories.ForEach(category =>
                    {
                        var result = vendorService.AddCategoryToVendor(vendor.Id,category).GetAwaiter().GetResult();
                    });

                    dummyMenu.ForEach(menu =>
                    {
                        menu.VendorId = vendor.Id;
                        var menuResult = menuService.AddVendorMenus(menu).GetAwaiter().GetResult();
                        menus.Add(menuResult);
                        var res = (menuResult.Id != null)? true : false;
                        loggerContext.LogInformation($"menu inserted with Success {res}");
                    });

                    loggerContext.LogInformation("Migration Mongo End");
                }

                if(foodTypeService.IfVendorFoodTypeCollectionExists() == 0)
                {
                    loggerContext.LogInformation("Migration for Food Type Started");
                    var result = foodTypeService.AddVendorFoodType(dummyFoodType).GetAwaiter().GetResult();
                    loggerContext.LogInformation("Migration for Food Type Finised");
                }

                if(cuisineTypeService.IfVendorCuisineDocumentExists() == 0)
                {
                    loggerContext.LogInformation("Migration for Cuisine Type Started");
                    var result = cuisineTypeService.AddVendorCuisineType(dummyCuisineType).GetAwaiter().GetResult();
                    loggerContext.LogInformation("Migration for Cuisine Type Finised");
                }

                if(!menuImageService.IfMenuImageDocumentExists())
                {
                    loggerContext.LogInformation("Migration for Menu Image Started");
                    var itemDictionary = new Dictionary<string, string>();
                    itemDictionary.Add("dosa", "Crispy single dosa with cutney");
                    itemDictionary.Add("idly", "2 idly with sambar");

                    var iamgeListToBeAdded = new List<MenuImageDto>();

                    //get image location
                    foreach(var file in Directory.GetFiles(Path.Combine(webHostService.WebRootPath, "DefaultMenuImages")))
                    {
                        iamgeListToBeAdded.Add(new MenuImageDto
                        {
                            FileName = file.Split("DefaultMenuImages\\")[1],
                            Active=true,
                            ImagePath = Path.Combine(webHostService.WebRootPath, "MenuImages",file)
                        });
                    }

                    iamgeListToBeAdded[0].ItemName = "Plain Dosa";
                    iamgeListToBeAdded[0].Description = itemDictionary["dosa"];
                    iamgeListToBeAdded[0].FileName = iamgeListToBeAdded[0].FileName.Split('.')[0] + "_" + menus.Find(m=>m.ItemName == "Plain Dosa").Id + "." + iamgeListToBeAdded[0].FileName.Split('.')[1];
                    byte[] byteDosa = File.ReadAllBytesAsync(Path.Combine(webHostService.WebRootPath, "DefaultMenuImages", "dosa.jpg")).GetAwaiter().GetResult();

                    iamgeListToBeAdded[1].ItemName = "Idly";
                    iamgeListToBeAdded[1].Description = itemDictionary["idly"];
                    iamgeListToBeAdded[1].FileName = iamgeListToBeAdded[1].FileName.Split('.')[0] + "_" + menus.Find(m => m.ItemName == "Idly").Id + "." + iamgeListToBeAdded[1].FileName.Split('.')[1];
                    byte[] byteIdly = File.ReadAllBytesAsync(Path.Combine(webHostService.WebRootPath, "DefaultMenuImages", "idly.png")).GetAwaiter().GetResult();

                    iamgeListToBeAdded.ForEach(image =>
                    {
                        var result = menuImageService.AddMenuImage(image).GetAwaiter().GetResult();

                        
                        //set the image path
                        string imgPath = Path.Combine(webHostService.WebRootPath, "MenuImages", image.FileName);

                        if(image.ItemName== "Plain Dosa")
                            System.IO.File.WriteAllBytes(imgPath, byteDosa);
                        else
                            System.IO.File.WriteAllBytes(imgPath, byteIdly);
                    });

                    

                    loggerContext.LogInformation("Migration for Menu Image Ended");
                }

            }
            return host;
        }
    }
}
