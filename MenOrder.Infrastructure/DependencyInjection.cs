using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Services;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace MenuManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfratructure(this IServiceCollection services,IConfiguration configuration)
        {
            //Redis Cache configuration
            ConfigurationOptions config = new ConfigurationOptions()
            {
                SyncTimeout = 5000, //checks for every 5 seconds
                EndPoints =
                {
                    { configuration.GetSection("RedisConfiguration:Server").Value,Convert.ToInt32(configuration.GetSection("RedisConfiguration:Port").Value) }
                },
                AbortOnConnectFail = false // this prevents that error
            };
            //var redis = ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection"));
            var redis = ConnectionMultiplexer.Connect(config);

            services.Configure<MongoDatabaseConfiguration>(configuration.GetSection("MongoOrderDBSettings"));

            //register services
            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<IRedisCacheBasketService, RedisCacheBasketService>();
            services.AddScoped<IMongoDBContext, MongoDBContext>();

            return services;
        }
    }
}
