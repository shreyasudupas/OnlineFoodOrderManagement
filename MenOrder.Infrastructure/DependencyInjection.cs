using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Services;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using MenuManagment.Domain.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository;
using MenuManagement.Core.Mongo.Interfaces;
using AutoMapper;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles;

namespace MenuManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfratructure(this IServiceCollection services,IConfiguration configuration)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            //Database registration
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<Core.Mongo.Interfaces.IMenuRepository, MenuRepository>();
            services.AddScoped<Core.Mongo.Interfaces.IVendorCartRepository, VendorCartRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorCuisineTypeRepository, VendorCuisineTypeRepository>();
            services.AddScoped<IVendorFoodTypeRepository, VendorFoodTypeRepository>();

            //mapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new VendorProfile());
                mc.AddProfile(new MenuProfile());
                mc.AddProfile(new CategoryProfile());
                mc.AddProfile(new VendorCuisineProfile());
                mc.AddProfile(new VendorFoodTypeProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
