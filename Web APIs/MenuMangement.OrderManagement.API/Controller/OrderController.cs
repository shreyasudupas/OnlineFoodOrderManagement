using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.AddOrderInformation;
using OrderManagement.Microservice.Core.Commands.OrderInformationCommand.UpdateOrderInformation;

namespace MenuMangement.OrderManagement.API.Controller
{
    public class OrderController : BaseController
    {
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
