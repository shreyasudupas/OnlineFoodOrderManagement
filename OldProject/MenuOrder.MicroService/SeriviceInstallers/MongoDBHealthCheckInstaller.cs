using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MenuOrder.MicroService.SeriviceInstallers
{
    public class MongoDBHealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks().AddMongoDb(
                mongodbConnectionString: configuration.GetValue<string>("MenuOrderDatabaseSettings:ConnectionString"),
                name: "MenuOrder.MicroService.MongoDBService",
                tags: new string[] { "db", "mongo" },
                failureStatus: HealthStatus.Degraded
                );
        }
    }
}
