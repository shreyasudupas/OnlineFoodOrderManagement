using VendorRegistration.Webjob.Core.Interfaces;

namespace VendorRegistration.WebJob
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IVendorRegistrationConsumerServices _vendorRegistrationConsumerServices;

        public Worker(ILogger<Worker> logger,
            IVendorRegistrationConsumerServices vendorRegistrationConsumerServices           
            )
        {
            _logger = logger;
            _vendorRegistrationConsumerServices = vendorRegistrationConsumerServices;
            //_processVendorRegistrationService = processVendorRegistrationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _vendorRegistrationConsumerServices.GetVendorRegistrationMessageFromQueue();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}