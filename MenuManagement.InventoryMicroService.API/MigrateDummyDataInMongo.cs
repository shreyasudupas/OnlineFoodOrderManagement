﻿using AutoMapper;
using MenuManagement.Core.Common.Enum;
using MenuManagement.Core.Common.Extension;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
                var loggerContext = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

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
                    VendorName = "Vendor 1"
                };

                var dummyCategories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        Id = "",
                        Name="Breakfast",
                        Active=true,
                        Description=""
                    },
                    new CategoryDto
                    {
                        Id = "",
                        Name="Lunch",
                        Active=true,
                        Description=""
                    },
                    new CategoryDto
                    {
                        Id = "",
                        Name="Dinner",
                        Active=true,
                        Description=""
                    },
                };

                var dummyMenu = new VendorMenuDto
                {
                    Id = "",
                    VendorId = "",
                    Disable = false,
                    Items = new List<MenuItemsDto>
                    {
                        new MenuItemsDto
                        {
                            Id= "",
                            ItemName = "Idly",
                            ImageLocation ="",
                            Price=20,
                            FoodType= dummyFoodType.TypeName,
                            Category = dummyCategories[0].Name,
                            Active=true,
                            Discount = 0
                        },
                        new MenuItemsDto
                        {
                            Id= "",
                            ItemName = "Plain Dosa",
                            ImageLocation ="",
                            Price=35,
                            FoodType= dummyFoodType.TypeName,
                            Category = dummyCategories[0].Name,
                            Active=true,
                            Discount = 10
                        },
                        new MenuItemsDto
                        {
                            Id= "",
                            ItemName = "Mini Lunch",
                            ImageLocation ="",
                            Price=45,
                            FoodType= dummyFoodType.TypeName,
                            Category = dummyCategories[1].Name,
                            Active=true,
                            Discount = 0
                        }
                    }

                };

                var isExists = vendorService.IfVendorCollectionExists();

                if (isExists == 0)
                {
                    loggerContext.LogInformation("Migration Mongo Started..");

                    var vendor = vendorService.AddVendorDocument(dummyVendors).GetAwaiter().GetResult();

                    dummyMenu.VendorId = vendor.Id;

                    dummyCategories.ForEach(category =>
                    {
                        var result = vendorService.AddCategoryToVendor(vendor.Id,category).GetAwaiter().GetResult();
                    });

                    var menu = menuService.AddVendorMenus(dummyMenu).GetAwaiter().GetResult();

                    loggerContext.LogInformation("Migration Mongo End");
                }

                if(foodTypeService.IfVendorFoodTypeCollectionExists() == 0)
                {
                    loggerContext.LogInformation("Migration of Food Type Started");
                    var result = foodTypeService.AddVendorFoodType(dummyFoodType).GetAwaiter().GetResult();
                    loggerContext.LogInformation("Migration of Food Type Finised");
                }

                if(cuisineTypeService.IfVendorCuisineDocumentExists() == 0)
                {
                    loggerContext.LogInformation("Migration of Cuisine Type Started");
                    var result = cuisineTypeService.AddVendorCuisineType(dummyCuisineType).GetAwaiter().GetResult();
                    loggerContext.LogInformation("Migration of Cuisine Type Finised");
                }

            }
            return host;
        }
    }
}
