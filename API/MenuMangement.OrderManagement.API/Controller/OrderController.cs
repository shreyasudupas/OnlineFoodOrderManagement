using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.FastOrderCancellation;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.OrderPlaced;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderStatusToDone;
using OrderManagement.Microservice.Core.Common.Model;
using OrderManagement.Microservice.Core.Querries.Orders.GetAllOrders;
using OrderManagement.Microservice.Core.Querries.Orders.GetOrderCount;
using OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders;

namespace MenuMangement.OrderManagement.API.Controller
{
    [Authorize]
    public class OrderController : BaseController
    {
        [HttpGet("/api/order/list")]
        public async Task<List<OrderInformationDto>> GetAllOrders([FromQuery] string userId)
        {
            return await Mediator.Send(new GetOrdersQuery { UserId = userId });
        }

        [HttpPost("/api/order")]
        public async Task<OrderInformationDto> AddOrderInformation([FromBody] OrderPlacedCommand orderPlacedCommand)
        {
            return await Mediator.Send(orderPlacedCommand);
        }

        [HttpPut("/api/order")]
        public async Task<OrderInformationDto> UpdateOrderInformation([FromBody] UpdateInformationModel updateInformation)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "");
            return await Mediator.Send(new UpdateOrderInformationCommand
            {
                OrderInfo = updateInformation.OrderInfo,
                Token = token
            });
        }

        [HttpPost("/api/order/list/status")]
        public async Task<List<OrderInformationDto>> GetOrdersBasedOnStatus([FromBody] GetVendorOrdersBasedOnOrderStatusQuery getVendorOrdersBasedOnOrderStatusQuery)
        {
            return await Mediator.Send(getVendorOrdersBasedOnOrderStatusQuery);
        }

        [HttpGet("/api/order/count")]
        //[AllowAnonymous]
        public async Task<GetOrderCountResponse> GetOrderCount([FromQuery]string vendorId)
        {
            return await Mediator.Send(new GetOrderCountQuery { VendorId = vendorId });
        }

        [HttpPost("/api/order/fastCancellation")]
        public async Task<OrderInformationDto> FastOrderCancellation([FromBody] OrderInformationDto orderInformation)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "");
            return await Mediator.Send(new FastOrderCancellationCommand
            {
                OrderInfo = orderInformation,
                Token = token
            });
        }

        [HttpPatch("/api/order/statusUpdate")]
        public async Task<bool> OrderStatusUpdateToDone([FromBody] UpdateOrderStatusToDoneCommand updateOrderStatusToDoneCommand)
        {
            return await Mediator.Send(updateOrderStatusToDoneCommand);
        }
    }
}
