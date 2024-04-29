using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;

namespace OrderManagement.Microservice.Core.Commands.OrderInformationCommand.FastOrderCancellation
{
    public class FastOrderCancellationCommand : IRequest<OrderInformationDto?>
    {
        public OrderInformationDto? OrderInfo { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    public class FastOrderCancellationCommandHandler : IRequestHandler<FastOrderCancellationCommand, OrderInformationDto?>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserPointsMuationGraphqlClient _userPointsMuationGraphqlClient;
        private readonly IMapper _mapper;
        private readonly ISignalROrderClientWrapper _signalROrderClientWrapper;

        public FastOrderCancellationCommandHandler(IOrderRepository orderRepository,
            IUserPointsMuationGraphqlClient userPointsMuationGraphqlClient,
            IMapper mapper,
            ISignalROrderClientWrapper signalROrderClientWrapper)
        {
            _orderRepository = orderRepository;
            _userPointsMuationGraphqlClient = userPointsMuationGraphqlClient;
            _mapper = mapper;
            _signalROrderClientWrapper = signalROrderClientWrapper;
        }

        public async Task<OrderInformationDto?> Handle(FastOrderCancellationCommand request, CancellationToken cancellationToken)
        {
            if(request.OrderInfo is not null)
            {
                //reverse the payment
                var isPayment = await SelectReversalPaymentMethod(request.OrderInfo, request.Token);

                //update the order repository
                return await UpdateUserCancellationOrder(request, isPayment);
            }
            else
            {
                throw new ArgumentNullException(nameof(request.OrderInfo));
            }
        }

        private async Task<bool> SelectReversalPaymentMethod(OrderInformationDto orderInformation,string token)
        {
            if(orderInformation.PaymentDetail.SelectedPayment.Equals("Reward"))
            {
                if(!string.IsNullOrEmpty(token))
                {
                    var paymentResult = await _userPointsMuationGraphqlClient.AdjustUserPoints(orderInformation.UserDetail.UserId,
                        orderInformation.PaymentDetail.Price, orderInformation.UserDetail.UserId);

                    if(!string.IsNullOrEmpty(paymentResult))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    throw new Exception("Token is Empty for payment reversal method");
                }
            }
            else
            {
                throw new NotImplementedException($"Other Payments Mode not Implemented");
            }
        }

        private async Task<OrderInformationDto?> UpdateUserCancellationOrder(FastOrderCancellationCommand request, bool isPayment)
        {
            OrderInformation order = new();
            order = _mapper.Map<OrderInformation>(request.OrderInfo);

            if (isPayment)
            {
                order.PaymentDetail.PaymentCredited = true;
                order.PaymentDetail.OrderCancelled = true;

                var orderResult = await _orderRepository.UpdateOrderInformation(order);
                var orderResponse = _mapper.Map<OrderInformationDto>(orderResult);

                if(orderResponse is not null)
                {
                    await SendCancellationUpdateToSignalRHub(orderResponse,request.Token);
                }
                return orderResponse;
            }
            else
            {
                throw new Exception("Payment Reversal Failed");
            }
        }

        private async Task SendCancellationUpdateToSignalRHub(OrderInformationDto orderInformation, string token)
        {
            if (orderInformation.CurrentOrderStatus == OrderStatusEnum.OrderCancelled.ToString())
            {
                var orderResult = await _signalROrderClientWrapper.PostCallAsync("recieveorder/cancel", orderInformation, token);
            }
        }
    }
}
