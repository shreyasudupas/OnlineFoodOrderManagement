using Common.Utility.BuisnessLayer;
using Common.Utility.BuisnessLayer.IBuisnessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.MicroService.Security.Handlers;
using Common.Utility.Tools.RedisCache.Interface;
using Common.Utility.Tools.RedisCache;

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
            services.AddScoped<IGetCacheBasketItemsService, GetCacheBasketItemsService>();

            //register all the Authorization Handlers here
            services.AddScoped<IAuthorizationHandler, CheckIfUserHandler>();
        }
    }
}
