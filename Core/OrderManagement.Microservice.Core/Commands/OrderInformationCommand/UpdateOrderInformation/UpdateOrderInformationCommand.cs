using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuOrder.Shared.Exceptions;
using Newtonsoft.Json;
using OrderManagement.Microservice.Core.Validators;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation
{
    public class UpdateOrderInformationCommand : IRequest<OrderInformationDto>
    {
        public OrderInformationDto OrderInfo { get; set; }
        public string Token { get; set; }
    }

    public class UpdateOrderInformationCommandHandler : IRequestHandler<UpdateOrderInformationCommand, OrderInformationDto>
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;
        private readonly ISignalROrderClientWrapper _signalROrderClientWrapper;

        public UpdateOrderInformationCommandHandler(IOrderRepository orderRepository
            , IMapper mapper
            ,ISignalROrderClientWrapper signalROrderClientWrapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _signalROrderClientWrapper = signalROrderClientWrapper;
        }

        public async Task<OrderInformationDto?> Handle(UpdateOrderInformationCommand request, CancellationToken cancellationToken)
        {
            var validationUpdate = new UpdateOrderValidator();
            var validationResult = await validationUpdate.ValidateAsync(request.OrderInfo);

            if(validationResult.IsValid)
            {
                var mapFromDtoModel = _mapper.Map<OrderInformation>(request.OrderInfo);
                var orderDetails = await _orderRepository.UpdateOrderInformation(mapFromDtoModel);

                var response = _mapper.Map<OrderInformationDto>(orderDetails);

                if(response is not null)
                {
                    await SendCancellationUpdateToSignalRHub(response, request.Token);
                }
                return response;
            } 
            else
            {
                var errorMessage = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                var errors = JsonConvert.SerializeObject(errorMessage, Formatting.Indented);
                throw new ValidationException(errors);
            }
        }

        private async Task SendCancellationUpdateToSignalRHub(OrderInformationDto orderInformation,string token)
        {
            if(orderInformation.CurrentOrderStatus == OrderStatusEnum.OrderCancelled.ToString())
            {
                var orderResult = await _signalROrderClientWrapper.PostCallAsync("recieveorder/cancel", orderInformation, token);
            }
        }
    }
}
