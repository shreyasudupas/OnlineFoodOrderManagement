using MenuManagement_IdentityServer.Service;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Register service
            services.AddScoped<IUserAdministrationManager, UserAdministrationManager>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<IClientService, ClientService>();

        }
    }
}
