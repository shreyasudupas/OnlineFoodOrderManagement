using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders
{
    public class GetVendorOrdersBasedOnOrderStatusQuery : IRequest<List<OrderInformationDto>>
    {
        public GetVendorByStatusRecord VendorStatus { get; set; }
    }

    public class GetVendorOrdersBasedOnOrderStatusQueryHandler : IRequestHandler<GetVendorOrdersBasedOnOrderStatusQuery, List<OrderInformationDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVendorOrdersBasedOnOrderStatusQueryHandler> _logger;

        public GetVendorOrdersBasedOnOrderStatusQueryHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<GetVendorOrdersBasedOnOrderStatusQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<OrderInformationDto>> Handle(GetVendorOrdersBasedOnOrderStatusQuery request, CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                var ordersByVendor = await _orderRepository.GetOrderInformationBasedOnOrderStatus(request.VendorStatus.VendorId,
                    request.VendorStatus.OrderStatus.ToString());
                var orderDtos = _mapper.Map<List<OrderInformationDto>>(ordersByVendor);
                return orderDtos;
            }
            else
            {
                _logger.LogError("Request is null");
                throw new Exception("Get Vendor Orders Status request is null");
            }
        }
    }
}
