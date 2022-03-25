using MenOrder.Infrastructure;
using MenuManagement.Core;
using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MenuOrder.BasketMicroService.API
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
            //Cors
            services.AddCors(options=>
            {
                var origin = Configuration.GetValue<string>("BasketApiCors:ORIGIN_URL");
                var headers = Configuration.GetValue<string>("BasketApiCors:HEADERS");
                var methods = Configuration.GetValue<string>("BasketApiCors:METHODS");

                options.AddPolicy(name: "BasketAPI.MicroService.Cors",
                    builder=>
                    {
                        builder.WithOrigins(origin)
                                .WithHeaders(headers.Split(','))
                                .WithMethods(methods.Split(','));
                    });
            });

            services.AddCore();
            services.AddInfratructure(Configuration);
            services.AddSharedInjection();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    opt.Authority = Configuration.GetValue<string>("AuthenticationConfig:AUTHORITY");
                    opt.Audience = Configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
                    //opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    //{
                    //    ValidateAudience = false
                    //};
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuOrder.BasketMicroService.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MenuOrder.BasketMicroService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("BasketAPI.MicroService.Cors");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Register custom middleware
            app.RegisterMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
