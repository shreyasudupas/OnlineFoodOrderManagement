using AutoMapper;
using IdenitityServer.Core;
using IdenitityServer.Core.MapperProfiles;
using IdentityServer.API.AutoMapperProfile;
using IdentityServer.API.GraphQL.Mutation;
using IdentityServer.API.GraphQL.Query;
using IdentityServer.API.GraphQL.Types.OutputTypes;
using IdentityServer.API.Middleware;
using IdentityServer.Infrastruture;
using IdentityServer.Infrastruture.MapperProfiles;
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
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.API
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
            services.AddCors(options =>
            {
                var origin = Configuration.GetValue<string>("IdentityReactSpaCors:ORIGIN_URL");
                var headers = Configuration.GetValue<string>("IdentityReactSpaCors:HEADERS");
                var methods = Configuration.GetValue<string>("IdentityReactSpaCors:METHODS");

                options.AddPolicy(name: "IdentityReactSpa.Cors",
                    builder =>
                    {
                        builder.WithOrigins(origin.Split(','))
                                .WithHeaders(headers.Split(','))
                                .WithMethods(methods.Split(','));
                    });
            });

            //services.AddAutoMapper(typeof(Startup));
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RegisterProfile());
                mc.AddProfile(new UserProfileMapper());
                mc.AddProfile(new LoginProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews();
            services.AddControllers();
            services.AddCors(Configuration);
            services.AddInfrastructure(Configuration);

            services.AddGraphQLServer()
                .AddQueryType(q => q.Name("Query"))
                .AddTypeExtension<UserInformationExtensionType>()
                .AddTypeExtension<UserAddressType>()
                .AddMutationType(m=>m.Name("Mutation"))
                .AddTypeExtension<AddUserInformationExtensionType>();
                //.AddType<UserInformationOutputType>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //custom OPTIONS CORS
            //app.UseOptions();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer.API v1"));
            }

            app.UseCors("IdentityReactSpa.Cors");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGraphQL();

                endpoints.MapControllers();
            });
        }
    }
}
