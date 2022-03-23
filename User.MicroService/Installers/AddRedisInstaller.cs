using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Identity.MicroService.Installers
{
    public class AddRedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigurationOptions config = new ConfigurationOptions()
            {
                SyncTimeout = 5000,//checks for every 5 seconds
                EndPoints =
                {
                    {configuration.GetValue<string>("RedisConfiguration:Server"),configuration.GetValue<int>("RedisConfiguration:Port") }
                },
                AbortOnConnectFail = false // this prevents that error
            };
            //var redis = ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisConnection"));
            var redis = ConnectionMultiplexer.Connect(config);
            services.AddSingleton<IConnectionMultiplexer>(redis);
        }
    }
}
