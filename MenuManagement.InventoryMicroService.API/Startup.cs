using MenuManagement.Core;
using MenuManagement.Infrastructure;
using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API
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
                var origin = Configuration.GetValue<string>("InventoryApiCors:ORIGIN_URL");
                var headers = Configuration.GetValue<string>("InventoryApiCors:HEADERS");
                var methods = Configuration.GetValue<string>("InventoryApiCors:METHODS");

                options.AddPolicy(name: "Inventory.MicroService.Cors",
                    builder =>
                    {
                        builder.WithOrigins(origin)
                                .WithHeaders(headers.Split(','))
                                .WithMethods(methods.Split(','));
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuManagement.InventoryMicroService.API", Version = "v1" });
            });

            services.AddCore();
            services.AddInfratructure(Configuration);
            services.AddSharedInjection();

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
                    //opt.Audience = Configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        //ValidateAudience = false
                        ValidAudiences = audenceNames
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MenuManagement.InventoryMicroService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("Inventory.MicroService.Cors");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Adding Custom Middleware
            app.RegisterMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}