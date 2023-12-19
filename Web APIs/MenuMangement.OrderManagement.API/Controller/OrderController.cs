using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.AddOrderInformation;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation;
using OrderManagement.Microservice.Core.Querries.Orders.GetAllOrders;

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
        public async Task<OrderInformationDto> AddOrderInformation([FromBody] AddOrderInformationCommand addOrderInformationCommand)
        {
            return await Mediator.Send(addOrderInformationCommand);
        }

        [HttpPut("/api/order")]
        public async Task<OrderInformationDto> UpdateOrderInformation([FromBody] UpdateOrderInformationCommand updateOrderInformationCommand)
        {
            return await Mediator.Send(updateOrderInformationCommand);
        }
    }
}
