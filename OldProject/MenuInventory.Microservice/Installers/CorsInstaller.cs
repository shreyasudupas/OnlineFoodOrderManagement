using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuInventory.Microservice.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowMyOrigin", options => options.AllowAnyOrigin()
            //     .AllowAnyHeader()
            //     .AllowAnyMethod()
            //    );
            //});
            //services.AddCors();
        }
    }
}
