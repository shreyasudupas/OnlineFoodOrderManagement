using MenuManagement.Infrastruture.RabbitMqClient;
using MenuMangement.Infrastructure.HttpClient;
using MenuManagement.MessagingQueue.Core;
using MenuManagement.MessagingQueue.Core.Interfaces;
using MenuMangement.RabbitMqClient.Domain.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.AddRabbitMQInfrastruture();
        //services.AddHostedService<Worker>();
        services.AddVendorRegistrationCore();
        services.AddInfrastrutureHttpClient(context.Configuration);
    })
    .ConfigureAppConfiguration((context,config) =>
    {
        //var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //config.AddJsonFile($"appsettings.{enviroment}.json", optional: true);
        config.AddEnvironmentVariables();
    })
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
IServiceProvider serviceProvider = serviceScope.ServiceProvider;

var logger = serviceProvider.GetService<ILogger<Program>>();
try
{
    
    var rabbitMQMessageInitilize = serviceProvider.GetService<IVendorRegistrationConsumerServices>();

    if(rabbitMQMessageInitilize != null)
    {
        logger?.LogInformation("VendorRegistration Job has started");
        rabbitMQMessageInitilize?.GetVendorRegistrationMessageFromQueue();
    }
    else
        logger?.LogError("rabbitMQ Initilize instance is null");

}
catch(Exception ex)
{
    logger?.LogInformation("VendorRegistration Job has ended");
    logger.LogError($"Error occured in the Vendor Registartion Job Exception: {ex.Message}");
}

await host.RunAsync();

Console.ReadLine();
