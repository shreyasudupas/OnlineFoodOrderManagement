using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MenuOrder.MicroService.SeriviceInstallers
{
    public class ReddisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigurationOptions config = new ConfigurationOptions()
            {
                SyncTimeout = 60000,
                EndPoints =
                {
                    {"127.0.0.1",6379 }
                },
                AbortOnConnectFail = false // this prevents that error
            };
            //r redis = ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisConnection"));
            var redis = ConnectionMultiplexer.Connect(config);
            services.AddSingleton<IConnectionMultiplexer>(redis);
        }
    }
}
