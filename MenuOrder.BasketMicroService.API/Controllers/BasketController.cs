using MenuManagement.Core.Services.BasketService.Query;
using MenuOrder.Shared.Controller;
using MenuOrder.Shared.Models;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MenuOrder.BasketMicroService.API.Controllers
{
    [Authorize]
    public class BasketController : BaseController
    {
        private readonly IProfileUser _profileUser;
        public BasketController(IProfileUser profileUser)
        {
            _profileUser = profileUser;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetBasketInformation()
        {
            return await Mediator.Send(new GetBasketInformationQuery { Username = _profileUser.Username });
        }

        
        [HttpGet("/api/basket/getbasketcount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(ExceptionResponse))]
        public async Task<int> GetBasketCount()
        {
            return await Mediator.Send(new GetUserBasketItemCountQuery { Username = _profileUser.Username });
        }
    }
}
