using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Identity.MicroService.Installers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                connectionString: configuration.GetConnectionString("DBConnectionString1"),
                healthQuery: "SELECT 1",
                name: "Identity.Microservice.SQL",
                failureStatus: HealthStatus.Degraded,
                tags: new string[] { "db", "sql", "sqlserver" })
                .AddRedis(
                redisConnectionString:configuration["RedisConnection"],
                name: "Identity.Microservice.Redis ",
                tags:new string[] { "redis", "basket" },
                failureStatus:HealthStatus.Degraded
                );
        }
    }
}
