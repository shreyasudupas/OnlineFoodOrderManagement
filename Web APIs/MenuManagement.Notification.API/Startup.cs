using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDb.Shared.Persistance;
using Notification.Microservice.Core;
using Notification.Microservice.Core.Hub;
using Notification.Mongo.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Notification.API
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
            services.AddCors(options =>
            {
                var origin = Configuration.GetValue<string>("NotificationApiCors:ORIGIN_URL");
                var headers = Configuration.GetValue<string>("NotificationApiCors:HEADERS");
                var methods = Configuration.GetValue<string>("NotificationApiCors:METHODS");

                options.AddPolicy(name: "Notification.MicroService.Cors",
                    builder =>
                    {
                        builder.WithOrigins(origin)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuManagement.Notification.API", Version = "v1" });
            });

            services.AddNotificationCore();
            services.AddNotificationMongoInfrastructure(Configuration);
            services.AddSharedInjection();
            services.AddSharedMongoServices(Configuration);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    var audienceList = Configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
                    var splitAudienceList = audienceList.Split(',');
                    var audenceNames = new List<string>();

                    foreach (var a in splitAudienceList)
                    {
                        audenceNames.Add(a);
                    }

                    opt.Authority = Configuration.GetValue<string>("AuthenticationConfig:AUTHORITY");
                    opt.Audience = Configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudiences = audenceNames
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = (context) =>
                        {
                            var loggerFactory = context.HttpContext
                                .RequestServices
                                .GetRequiredService<ILoggerFactory>();

                            var logger = loggerFactory.CreateLogger(typeof(Startup));
                            logger.LogError(context.Exception, "Authentication Failed");

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MenuManagement.Notification.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("Notification.MicroService.Cors");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Adding Custom Middleware
            app.RegisterSharedMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notification");
                endpoints.MapControllers();
            });
        }
    }
}
