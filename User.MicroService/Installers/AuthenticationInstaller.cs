using Common.Utility.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.MicroService.Installers
{
    public class AuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Adding Authentication
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = $"https://{configuration["Auth0:Domain"]}/";
            //    options.Audience = configuration["Auth0:Audience"];
            //});
            //var token = configuration["Jwt:Secret"]?? "This is a test secret";

            services.AddJwtAuthentication(configuration);
        }
    }
}
