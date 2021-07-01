using MenuInventory.Microservice.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}
