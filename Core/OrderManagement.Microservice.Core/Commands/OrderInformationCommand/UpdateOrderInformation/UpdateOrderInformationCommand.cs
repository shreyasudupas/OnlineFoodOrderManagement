using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation
{
    public class UpdateOrderInformationCommand : IRequest<OrderInformationDto>
    {
        public OrderInformationDto OrderInfo { get; set; }
    }

    public class UpdateOrderInformationCommandHandler : IRequestHandler<UpdateOrderInformationCommand, OrderInformationDto>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public UpdateOrderInformationCommandHandler(IOrderRepository orderRepository
            , IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderInformationDto> Handle(UpdateOrderInformationCommand request, CancellationToken cancellationToken)
        {
            var mapFromDtoModel = _mapper.Map<OrderInformation>(request.OrderInfo);
            var orderDetails = await _orderRepository.UpdateOrderInformation(mapFromDtoModel);

            var response = _mapper.Map<OrderInformationDto>(orderDetails);
            return response;
        }
    }
}
