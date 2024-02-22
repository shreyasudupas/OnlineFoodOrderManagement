using OrderManagement.Microservice.Core;
using MenuOrder.Shared;
using MenuOrder.Shared.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MongoDb.Shared.Persistance;
using OrderManagement.Mongo.Persistance;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                    .AddEnvironmentVariables()
                                    .Build();

builder.Services.AddCors(options =>
{
    var origin = configuration.GetValue<string>("OrderManagementApiCors:ORIGIN_URL");
    var headers = configuration.GetValue<string>("OrderManagementApiCors:HEADERS");
    var methods = configuration.GetValue<string>("OrderManagementApiCors:METHODS");

    options.AddPolicy(name: "OrderManagement.MicroService.Cors",
        builder =>
        {
            builder.WithOrigins(origin)
                    .WithHeaders(headers.Split(','))
                    .WithMethods(methods.Split(','));
        });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuManagement.CartInformationMicroService.API", Version = "v1" });
});

builder.Services.OrderManagementCore();
builder.Services.AddOrderManagementPersistance(configuration);
builder.Services.AddSharedInjection();
builder.Services.AddSharedMongoServices(configuration);

builder.Services.AddAuthentication("Bearer")
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

app.UseCors("OrderManagement.MicroService.Cors");

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