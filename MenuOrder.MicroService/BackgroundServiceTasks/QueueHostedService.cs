using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MenuOrder.MicroService.BackgroundServiceTasks
{
    public class QueueHostedService : BackgroundService
    {
        public IBackgroundTaskQueue TaskQueue { get; }
        private readonly ILogger<QueueHostedService> _logger;

        public QueueHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueueHostedService> logger)
        {
            TaskQueue = taskQueue;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queue Hosted Service has started");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueBackgroundWorkItem(stoppingToken);
                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,"Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
