using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using MenuOrder.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderManagement.Microservice.Core.Validators;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders
{
    public class GetVendorOrdersBasedOnOrderStatusQuery : IRequest<List<OrderInformationDto>>
    {
        public string VendorId { get; set; } = string.Empty;
        public string[] VendorStatus { get; set; } = Array.Empty<string>();
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
            GetVendorOrdersBasedOnOrderStatusQueryValidator validator = new GetVendorOrdersBasedOnOrderStatusQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                if (request is not null)
                {
                    List<OrderInformationDto> result = new();

                    var ordersByVendor = await _orderRepository.GetOrderInformationBasedOnOrderStatus(request.VendorId,
                            request.VendorStatus);
                    var orderDtos = _mapper.Map<List<OrderInformationDto>>(ordersByVendor);

                    return orderDtos;
                }
                else
                {
                    _logger.LogError("Request is null");
                    throw new Exception("Get Vendor Orders Status request is null");
                }
            }
            else
            {
                var error = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new ValidationException(JsonConvert.SerializeObject(error));
            }
            
        }
    }
}
