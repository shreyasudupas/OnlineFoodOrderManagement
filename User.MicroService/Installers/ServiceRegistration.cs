using Common.Utility.BuisnessLayer;
using Common.Utility.BuisnessLayer.IBuisnessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.MicroService.Security.Handlers;

namespace Identity.MicroService.Installers
{
    public class ServiceRegistration : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //register services
            //services.AddScoped<IUser, UserBuisness>();

            //register for properties
            services.AddScoped<IProfileUser, ProfileUser>();

            //register all the Authorization Handlers here
            services.AddScoped<IAuthorizationHandler, CheckIfUserHandler>();
        }
    }
}
