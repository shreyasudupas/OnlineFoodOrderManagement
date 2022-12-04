using AutoMapper;
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

                var vendorContext = scope.ServiceProvider.GetRequiredService<IVendorRepository>();
                var loggerContext = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var dummyVendors = new List<VendorDto>
                {
                    new VendorDto
                    {
                        Id = "",
                        AddressLine1="sample",
                        AddressLine2=null,
                        Area="Kathreguppe",
                        Category="Dining",
                        City="Bengaluru",
                        Coordinates = new CoordinatesDto
                        {
                            Latitude = 12.2334,
                            Longitute =92.1212
                        },
                        State="Karnataka",
                        Rating=4,
                        Type="Vegetarian",
                        VendorDescription="sample",
                        VendorName = "Vendor 1"
                    }
                };

                var isExists = vendorContext.IfVendorCollectionExists();

                if (isExists == 0)
                {
                    loggerContext.LogInformation("Migration Mongo Started..");

                    var ven = vendorContext.AddVendorDocuments(dummyVendors).Result;

                    loggerContext.LogInformation("Migration Mongo End");
                }

            }
            return host;
        }
    }
}
