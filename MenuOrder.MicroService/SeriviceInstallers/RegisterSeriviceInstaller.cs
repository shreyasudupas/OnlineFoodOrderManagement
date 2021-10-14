using MenuOrder.MicroService.BackgroundServiceTasks;
using MenuOrder.MicroService.Data;
using MenuOrder.MicroService.Data.Context;
using MenuOrder.MicroService.Helper;
using MenuOrder.MicroService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuOrder.MicroService.SeriviceInstallers
{
    public class RegisterSeriviceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDatabaseConfiguration>(configuration.GetSection("MenuOrderDatabaseSettings"));
            
            services.AddHostedService<QueueHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<HttpClientCrudService>();
            services.AddScoped<OrderRepository>();
        }
    }
}
