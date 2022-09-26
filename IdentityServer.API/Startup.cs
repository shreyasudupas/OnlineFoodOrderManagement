using AutoMapper;
using IdenitityServer.Core;
using IdenitityServer.Core.MapperProfiles;
using IdenitityServer.Core.MutationResolver;
using IdenitityServer.Core.QueryResolvers;
using IdentityServer.API.AutoMapperProfile;
using IdentityServer.API.GraphQL.Mutation;
using IdentityServer.API.GraphQL.Query;
using IdentityServer.API.GraphQL.Types.OutputTypes;
using IdentityServer.API.Middleware;
using IdentityServer.Infrastruture;
using IdentityServer.Infrastruture.MapperProfiles;
using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            services.AddSharedInjection();

            services.AddHttpContextAccessor();

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = "https://localhost:5006",
                           ValidAudience = "idsAPI"

                       };
                   options.Events = new JwtBearerEvents
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

            services.AddAuthorization(p => 
                p.AddPolicy("isAdmin", policy => policy.RequireClaim("role", "admin"))
                );

            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(q => q.Name("Query"))
                .AddTypeExtension<UserInformationExtensionType>()
                .AddTypeExtension<UserAddressType>()
                .AddTypeExtension<UserInformationListExtensionType>()
                .AddTypeExtension<RolesExtensionType>()
                .AddTypeExtension<ApiScopeQueryExtensionType>()
                .AddTypeExtension<ClientQueryExtensionType>()
                .AddMutationType(m=>m.Name("Mutation"))
                .AddTypeExtension<SaveUserInformationExtensionType>()
                .AddTypeExtension<AddModifyUserAddressExtensionType>()
                .AddTypeExtension<AddRoleExtensionType>()
                .AddTypeExtension<DeleteRoleExtensionType>()
                .AddTypeExtension<SaveRoleExtensionType>()
                .AddTypeExtension<ApiScopeMutationExtensionType>()
                .AddTypeExtension<ClientMutationExtensionType>()
                .RegisterService<AddModifyUserAddressResolver>()
                .RegisterService<IProfileUser>()
                .RegisterService<GetUserListResolver>()
                .RegisterService<GetUserRolesResolver>()
                .RegisterService<MutationRoleResolver>()
                .RegisterService<ClientQueryResolver>()
                .RegisterService<ClientMutationResolver>()
                ;
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.RegisterSharedMiddleware();

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
