using BasketService.MicroService.BuisnessLayer;
using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using BasketService.MicroService.Extensions;
using Common.Utility.Security;
using Common.Utility.Tools.RedisCache;
using Common.Utility.Tools.RedisCache.Interface;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace BasketService.MicroService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketService.MicroService", Version = "v1" });
            });

            //Add Authnetication Extension
            services.AddJwtAuthentication(Configuration);

            //Redis Cache configuration
            ConfigurationOptions config = new ConfigurationOptions()
            {
                SyncTimeout = 5000, //checks for every 5 seconds
                EndPoints =
                {
                    {Configuration.GetValue<string>("RedisConfiguration:Server"),Configuration.GetValue<int>("RedisConfiguration:Port") }
                },
                AbortOnConnectFail = false // this prevents that error
            };
            //var redis = ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection"));
            var redis = ConnectionMultiplexer.Connect(config);

            //Register services
            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IGetCacheBasketItemsService, GetCacheBasketItemsService>();

            services.AddHealthChecks().AddRedis(
                redisConnectionString: Configuration["RedisConnection"],
                name: "BasketService.Microservice.Redis ",
                tags: new string[] { "redis", "basket" },
                failureStatus: HealthStatus.Degraded);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketService.MicroService v1"));
            }

            app.UseHttpsRedirection();

            //app.UseCors(builder =>
            //{
            //    builder
            //    .WithOrigins("http://localhost:4200")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials();
            //});

            app.UseRouting();

            app.UseAuthentication();

            app.InstallCustomExceptionMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });
        }
    }
}
