using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Identity.MicroService.Installers
{
    public class AddRedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisConnection"));
            services.AddSingleton<IConnectionMultiplexer>(redis);
        }
    }
}
