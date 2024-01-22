using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDb.Shared.Persistance;
using Notification.Microservice.Core;
using Notification.Mongo.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;
using MenuMangement.Infrastructure.HttpClient;


var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                    .AddEnvironmentVariables()
                                    .Build();

    // Add services to the container.
    builder.Services.AddCors(options =>
    {
        var origin = configuration.GetValue<string>("NotificationApiCors:ORIGIN_URL");
        var headers = configuration.GetValue<string>("NotificationApiCors:HEADERS");
        var methods = configuration.GetValue<string>("NotificationApiCors:METHODS");

        options.AddPolicy(name: "Notification.MicroService.Cors",
            builder =>
            {
                builder.WithOrigins(origin)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            });
    });

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuManagement.Notification.API", Version = "v1" });
    });

    builder.Services.AddNotificationCore();
    builder.Services.AddNotificationMongoInfrastructure(configuration);
    builder.Services.AddSharedInjection();
    builder.Services.AddSharedMongoServices(configuration);
    builder.Services.AddInfrastrutureHttpClient(configuration);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", opt =>
    {
        var audienceList = configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
        var splitAudienceList = audienceList.Split(',');
        var audenceNames = new List<string>();

        foreach (var a in splitAudienceList)
        {
            audenceNames.Add(a);
        }

        opt.Authority = configuration.GetValue<string>("AuthenticationConfig:AUTHORITY");
        opt.Audience = configuration.GetValue<string>("AuthenticationConfig:AUDIENCE");
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

                var logger = loggerFactory.CreateLogger(typeof(Program));
                logger.LogError(context.Exception, "Authentication Failed");

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("Notification.MicroService.Cors");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//Adding Custom Middleware
app.RegisterSharedMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();