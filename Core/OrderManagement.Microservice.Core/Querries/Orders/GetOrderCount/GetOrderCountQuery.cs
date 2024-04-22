using MediatR;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetOrderCount
{
    public class GetOrderCountQuery : IRequest<GetOrderCountResponse>
    {
        public string VendorId { get; set; } = string.Empty;
    }

    public class GetOrderCountQueryHandler : IRequestHandler<GetOrderCountQuery, GetOrderCountResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderCountQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetOrderCountResponse> Handle(GetOrderCountQuery request, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(request.VendorId))
            {
                var result = await _orderRepository.GetOrderCountByVendorId(request.VendorId);
                return new GetOrderCountResponse(result.OrderPlacedCount,result.OrderInProgressCount,result.OrderCancelledCount);
            }
            else
            {
                throw new ArgumentNullException(nameof(request.VendorId));
            }
        }
    }
}
