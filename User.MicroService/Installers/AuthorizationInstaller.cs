using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.MicroService.Security.Requirments;

namespace Identity.MicroService.Installers
{
    public class AuthorizationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllowUserAccess", policy => policy.Requirements.Add(new UserRequirement("User")));
            });
        }
    }
}
