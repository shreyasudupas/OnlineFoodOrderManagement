using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.MicroService.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MenuDatabase.Data.Database.MenuOrderManagementContext>(options =>
                 options.UseSqlServer(
                     configuration.GetConnectionString("DBConnectionString"))
             );
        }
    }
}
