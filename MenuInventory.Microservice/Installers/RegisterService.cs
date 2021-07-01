using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuInventory.Microservice.Installers
{
    public class RegisterService : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //register for properties
            //services.AddScoped<IProfileUser, ProfileUser>();

            //register services
            //services.AddScoped<IUser, UserBuisness>();
            //services.AddScoped<IVendorBL, VendorBL>();
            //services.AddScoped<IMenuBL, MenuBL>();
        }
    }
}
