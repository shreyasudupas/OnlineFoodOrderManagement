using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetAllVendorOrderSummary
{
    public class GetAllVendorOrderSummaryQuery : IRequest<List<VendorOrderInformationSummary>>
    {
    }

    public class GetAllVendorOrderSummaryQueryHandler : IRequestHandler<GetAllVendorOrderSummaryQuery, List<VendorOrderInformationSummary>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllVendorOrderSummaryQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<VendorOrderInformationSummary>> Handle(GetAllVendorOrderSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetVendorOrderInformationSummary();
        }
    }
}
