using AutoMapper;
using HotChocolate.Types;
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
using MenuManagement.Infrastruture.RabbitMqClient;
using MenuManagement.MessagingQueue.Core;
using MenuMangement.Infrastructure.HttpClient;
using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
                mc.AddProfile(new VendorIdMappingProfile());
                mc.AddProfile(new CommonProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews();
            services.AddControllers();
            services.AddCors(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddSharedInjection();

            //for messaging queues service registrations
            services.AddVendorRegistrationCore();
            services.AddRabbitMQInfrastruture();
            services.AddInfrastrutureHttpClient(Configuration);

            services.AddHttpContextAccessor();

            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = "https://localhost:5006",
                           ValidAudience = "inventory",
                           ValidateAudience = true,
                           SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                           {
                               var jwt = new JwtSecurityToken(token);

                               return jwt;
                           }

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
                       },
                       OnChallenge = (context) =>
                       {
                           return Task.CompletedTask;
                       }
                   };
                });

            services.AddAuthorization(p => 
                p.AddPolicy("isAdmin", policy => policy.RequireRole(new string[] { "admin" }))
                );

            services.AddGraphQLServer()
                .AddQueryType(q => q.Name("Query"))
                .AddType<UserInformationExtensionType>()
                .AddType<UserAddressType>()
                .AddType<UserInformationListExtensionType>()
                .AddType<RolesExtensionType>()
                .AddType<ApiScopeQueryExtensionType>()
                .AddType<ClientQueryExtensionType>()
                .AddType<AddressQueryExtensionType>()
                .AddMutationType(m => m.Name("Mutation"))
                .AddType<SaveUserInformationExtensionType>()
                .AddType<ApiScopeMutationExtensionType>()
                .AddType<ClientMutationExtensionType>()
                .AddType<AddressExtensionType>()
                .AddType<UserProfileExtensionType>()
                .AddType<UploadType>()
                .AddType<ApiResourceQueryExtensionType>()
                .AddType<ApiResourceMutationExtensionType>()
                .AddType<IdentityResourceMutationExtensionType>()
                .AddType<IdentityResourceQueryExtensionType>()
                //.RegisterService<AddModifyUserAddressResolver>()
                .RegisterService<UserProfileResolver>()
                .RegisterService<IProfileUser>()
                .RegisterService<GetUserListResolver>()
                .RegisterService<GetUserRolesResolver>()
                .RegisterService<MutationRoleResolver>()
                .RegisterService<ClientQueryResolver>()
                .RegisterService<ClientMutationResolver>()
                .RegisterService<AddressQueryResolver>()
                .RegisterService<AddressMutationResolver>()
                .RegisterService<IWebHostEnvironment>()
                .RegisterService<ApiResourceQueryResolver>()
                .RegisterService<ApiResourceMutationResolver>()
                .RegisterService<IdenitityResourceQueryResolver>()
                .RegisterService<IdentityResourcesMutationResolver>()
                .AddAuthorization()
                //.AddMutationConventions()
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

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", @"images")),
                RequestPath = new PathString("/ids-images")
            });

            app.UseCors("IdentityReactSpa.Cors");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.RegisterSharedMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });
        }
    }
}
