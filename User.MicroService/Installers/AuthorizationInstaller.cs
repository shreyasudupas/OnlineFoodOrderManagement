using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.MicroService.Security.Requirments;

namespace User.MicroService.Installers
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
