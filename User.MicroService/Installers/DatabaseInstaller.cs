using Identity.MicroService.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<MenuDatabase.Data.Database.MenuOrderManagementContext>(options =>
            //     options.UseSqlServer(
            //         configuration.GetConnectionString("DBConnectionString"))
            // );
            services.AddDbContext<UserContext>(options =>
                 options.UseSqlServer(
                     configuration.GetConnectionString("DBConnectionString1"))
             );
        }
    }
}
