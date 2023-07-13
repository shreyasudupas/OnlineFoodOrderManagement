using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Microservice.Core.Commands.CartInformationCommand;
using OrderManagement.Microservice.Core.Querries.CartInformationQuery;

namespace MenuMangement.OrderManagement.API.Controller
{
    [Authorize]
    public class CartInformationController : BaseController
    {
        [HttpGet("/api/cartInformation")]
        public async Task<CartInformationDto> GetActiveCartInformation([FromQuery]string UserId)
        {
            return await Mediator.Send(new GetCartInformationQuery
            {
                UserId = UserId
            });
        }

        [HttpPost("/api/cartInformation")]
        public async Task<CartInformationDto> AddCartInformation([FromBody] AddCartInformationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("/api/cartInformation")]
        public async Task<CartInformationDto?> UpdateCartInformation([FromBody] UpdateCartInformationCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
