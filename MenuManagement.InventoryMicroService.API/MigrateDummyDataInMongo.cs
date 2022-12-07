using AutoMapper;
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
                var menuService = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var loggerContext = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var dummyVendors = new VendorDto
                {
                    Id = "",
                    AddressLine1 = "sample",
                    AddressLine2 = null,
                    Area = "Kathreguppe",
                    Category = "Dining",
                    City = "Bengaluru",
                    Coordinates = new CoordinatesDto
                    {
                        Latitude = 12.2334,
                        Longitute = 92.1212
                    },
                    State = "Karnataka",
                    Rating = 4,
                    Type = "Vegetarian",
                    VendorDescription = "sample",
                    VendorName = "Vendor 1"
                };

                var dummyMenu = new MenuDto
                {
                    Id="",
                    VendorId = "",
                    Disable = false,
                    Items = new List<MenuItemsDto>
                    {
                        new MenuItemsDto
                        {
                            Id= "",
                            Name = "test menu 1",
                            PictureLocation ="",
                            Price=20,
                            Type= MenuTypeEnum.Breakfast.GetEnumDescription(),
                            Discount = 0
                        },
                        new MenuItemsDto
                        {
                            Id="",
                            Name = "test menu 2",
                            PictureLocation ="",
                            Price=45,
                            Type= MenuTypeEnum.Lunch.GetEnumDescription(),
                            Discount = 0
                        },
                        new MenuItemsDto
                        {
                            Id = "",
                            Name = "test menu 3",
                            PictureLocation ="",
                            Price=25,
                            Type= MenuTypeEnum.Dinner.GetEnumDescription(),
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

                    var menu = menuService.AddMenu(dummyMenu).GetAwaiter().GetResult();

                    loggerContext.LogInformation("Migration Mongo End");
                }

            }
            return host;
        }
    }
}
