using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Enum;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.OrderPlaced;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation;
using OrderManagement.Microservice.Core.Querries.Orders.GetAllOrders;
using OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders;

namespace MenuMangement.OrderManagement.API.Controller
{
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
        public async Task<OrderInformationDto> UpdateOrderInformation([FromBody] UpdateOrderInformationCommand updateOrderInformationCommand)
        {
            return await Mediator.Send(updateOrderInformationCommand);
        }

        [HttpPost("/api/order/list/status")]
        public async Task<List<OrderInformationDto>> GetOrdersBasedOnStatus([FromBody] GetVendorOrdersBasedOnOrderStatusQuery getVendorOrdersBasedOnOrderStatusQuery)
        {
            return await Mediator.Send(getVendorOrdersBasedOnOrderStatusQuery);
        }
    }
}
