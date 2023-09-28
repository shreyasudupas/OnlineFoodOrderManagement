using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.AddOrderInformation
{
    public class AddOrderInformationCommand : IRequest<OrderInformationDto>
    {
        public OrderInformationDto OrderInfo { get; set; }
    }

    public class AddOrderInformationCommandHandler : IRequestHandler<AddOrderInformationCommand, OrderInformationDto>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public AddOrderInformationCommandHandler(IOrderRepository orderRepository
            , IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderInformationDto> Handle(AddOrderInformationCommand request, CancellationToken cancellationToken)
        {
            var mapFromDtoModel = _mapper.Map<OrderInformation>(request.OrderInfo);
            var orderDetails = await _orderRepository.AddOrderInformation(mapFromDtoModel);

            var response = _mapper.Map<OrderInformationDto>(orderDetails);
            return response;
        }
    }
}
