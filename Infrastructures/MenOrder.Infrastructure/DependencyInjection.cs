using MongoDb.Infrastructure.Persistance.Services;
using Inventory.Microservice.Core.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using MongoDb.Infrastructure.Persistance.Persistance.MongoDatabase.Repository;
using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.MappingProfile;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;

namespace MongoDb.Infrastructure.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMongoInfratructure(this IServiceCollection services,IConfiguration configuration)
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

            

            //register services
            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<IRedisCacheBasketService, RedisCacheBasketService>();

            //Database registration
            services.AddScoped<IVendorsMenuRepository, VendorsMenuRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorCuisineTypeRepository, VendorCuisineTypeRepository>();
            services.AddScoped<IVendorFoodTypeRepository, VendorFoodTypeRepository>();
            services.AddScoped<IMenuImagesRepository,MenuImagesRepository>();

            //mapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new VendorProfile());
                mc.AddProfile(new MenuProfile());
                //mc.AddProfile(new CategoryProfile());
                mc.AddProfile(new VendorCuisineProfile());
                mc.AddProfile(new VendorFoodTypeProfile());
                mc.AddProfile(new MenuImageProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
