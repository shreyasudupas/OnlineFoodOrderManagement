using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.OrderPlaced
{
    public class OrderPlacedCommand : IRequest<OrderInformationDto>
    {
        public OrderInformationDto OrderInfo { get; set; }
    }

    public class OrderPlacedCommandHandler : IRequestHandler<OrderPlacedCommand, OrderInformationDto>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public OrderPlacedCommandHandler(IOrderRepository orderRepository
            , IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderInformationDto> Handle(OrderPlacedCommand request, CancellationToken cancellationToken)
        {
            var mapFromDtoModel = _mapper.Map<OrderInformation>(request.OrderInfo);
            var getUiBasedOrderId = await _orderRepository.GetNextUIBasedOrderNumber(request.OrderInfo.VendorDetail.VendorId);

            mapFromDtoModel.UIOrderNumber = getUiBasedOrderId;
            mapFromDtoModel.CreatedDate = DateTime.Now;
            mapFromDtoModel.OrderStatusDetails.OrderPlaced = DateTime.Now;
            mapFromDtoModel.CurrentOrderStatus = OrderStatusEnum.OrderPlaced.ToString();

            var orderDetails = await _orderRepository.AddOrderInformation(mapFromDtoModel);

            var response = _mapper.Map<OrderInformationDto>(orderDetails);
            return response;
        }
    }
}
