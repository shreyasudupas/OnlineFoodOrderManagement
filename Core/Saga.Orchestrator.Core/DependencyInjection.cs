using MediatR;
using MenuMangement.HttpClient.Domain.Orchestrator;
using Microsoft.Extensions.DependencyInjection;
using Saga.Orchestrator.Core.Orchestrator;
using System.Reflection;

namespace Saga.Orchestrator.Core
{
    public static class DependencyInjection
    {
        public static void SagaOrchestratorCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IPaymentOrchestrator, PaymentOrchestrator>();
        }
    }
}
