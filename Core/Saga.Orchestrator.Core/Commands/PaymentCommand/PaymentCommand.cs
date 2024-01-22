using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuMangement.HttpClient.Domain.Dtos;
using MenuMangement.HttpClient.Domain.Orchestrator;

namespace Saga.Orchestrator.Core.Commands.PaymentCommand
{
    public class PaymentCommand : IRequest<PaymentProcessDto>
    {
        public PaymentInformationRecord paymentInformation { get; set; }
    }

    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, PaymentProcessDto>
    {
        private readonly IPaymentOrchestrator _paymentOrchestrator;

        public PaymentCommandHandler(IPaymentOrchestrator paymentOrchestrator)
        {
            _paymentOrchestrator = paymentOrchestrator;
        }

        public async Task<PaymentProcessDto> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            return await _paymentOrchestrator.PaymentProcess(request.paymentInformation);
        }
    }
}
