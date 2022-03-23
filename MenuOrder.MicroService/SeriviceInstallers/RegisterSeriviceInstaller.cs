using Common.Mongo.Database.Data.BaseContext;
using Common.Mongo.Database.Data.Context;
using Common.Mongo.Database.Helper;
using Common.Mongo.Database.Models;
using MenuOrder.MicroService.BackgroundServiceTasks;
using MenuOrder.MicroService.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuOrder.MicroService.SeriviceInstallers
{
    public class RegisterSeriviceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Register MongoDB Connection string
            services.Configure<MongoDatabaseConfiguration>(configuration.GetSection("MenuOrderDatabaseSettings"));
            
            //Register services
            services.AddScoped<HttpClientCrudService>();
            services.AddScoped<MenuConfigurationService>();
            services.AddHostedService<QueueHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            //Register Repository
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<MenuRepository>();
            services.AddScoped<OrderRepository>();
        }
    }
}
