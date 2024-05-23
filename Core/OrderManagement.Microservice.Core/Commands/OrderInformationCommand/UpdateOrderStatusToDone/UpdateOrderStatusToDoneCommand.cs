using MediatR;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderStatusToDone
{
    public class UpdateOrderStatusToDoneCommand : IRequest<bool>
    {
        public string OrderId { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusToDoneCommandHandler : IRequestHandler<UpdateOrderStatusToDoneCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusToDoneCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(UpdateOrderStatusToDoneCommand request, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(request.OrderId))
            {
                var result = await _orderRepository.UpdateOrderStatusToOrderDoneBasedOnOrderId(request.OrderId);
                return result;
            }
            else
            {
                throw new ArgumentNullException(request.OrderId);
            }
        }
    }
}
