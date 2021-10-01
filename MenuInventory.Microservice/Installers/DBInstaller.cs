using MenuInventory.Microservice.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MenuInventory.Microservice.Installers
{
    public class DBInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<MenuDatabase.Data.Database.MenuOrderManagementContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("DBConnectionString"))
            //);
            services.AddDbContext<MenuInventoryContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DBConnectionString1"))
            );

            services.AddHealthChecks().AddMongoDb(
                mongodbConnectionString:configuration.GetValue<string>("MenuOrderDatabaseSettings:ConnectionString"),
                name: "MenuInventory.Microservice.MongoDBService",
                tags:new string[] {"db","mongo"},
                failureStatus:HealthStatus.Degraded 
                );
        }
    }
}
