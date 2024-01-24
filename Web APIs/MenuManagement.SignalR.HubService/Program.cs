
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SignalRHub.Base.Infrastructure;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Hubs;
using MenuMangement.Infrastructure.HttpClient;
using MenuMangement.HttpClient.Domain.Models;
using SignalRHub.Base.Infrastructure.NotificationFactory.FactoryMethod;

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
    var origin = configuration.GetValue<string>("SignalRService:ORIGIN_URL");
    var headers = configuration.GetValue<string>("SignalRService:HEADERS");
    var methods = configuration.GetValue<string>("SignalRService:METHODS");

    options.AddPolicy(name: "SignalR.Cors",
        builder =>
        {
            builder.WithOrigins(origin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddSignalRInfrastructure();
builder.Services.AddInfrastrutureHttpClient(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
            },
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hubs")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("SignalR.Cors");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.MapGet("/api/notification/count", async (HttpRequest request, INotificationFactory notificationFactory) =>
{
    var fromUserId = request.Query["fromUserId"];
    var toUserId = request.Query["toUserId"];
    var isSendAll = Convert.ToBoolean(request.Query["isSendAll"]);
    var notificationCount = Convert.ToInt32(request.Query["count"]);

    //await notificationHubService.SendNotificationToConnectedUsers(toUserId, role, notificationCount);
    await notificationFactory.SendNotification(new NotificationSignalRRequest
    {
        FromUserId = fromUserId,
        ToUserId = toUserId,
        isSendAll = isSendAll,
        NotificationCount = notificationCount
    });

    Results.Ok();
});

app.Run();
