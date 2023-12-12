using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using Saga.Orchestrator.Core.Interfaces.Orchestrator;
using Saga.Orchestrator.Core.Models.Dtos;

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
