using MenuManagement.Core.Services.BasketService.Query;
using MenuOrder.Shared.Controller;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MenuOrder.BasketMicroService.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IProfileUser _profileUser;
        public BasketController(IProfileUser profileUser)
        {
            _profileUser = profileUser;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetBasketInformation([FromQuery]GetBasketInformationQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpGet("/api/basket/getbasketcount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetBasketCount()
        {
            var r = _profileUser.UserId;
            return "true";
        }
    }
}
