using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Querries.Orders.GetAllOrders;

public class GetOrdersQuery : IRequest<List<OrderInformationDto>>
{
    public string UserId { get; set; }
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderInformationDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderInformationDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllOrdersBasedOnUserId(request.UserId);
        var ordersDto = _mapper.Map<List<OrderInformationDto>>(orders);
        return ordersDto;
    }
}
