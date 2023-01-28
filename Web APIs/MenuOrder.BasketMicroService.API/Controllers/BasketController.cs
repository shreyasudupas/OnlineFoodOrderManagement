using MenuManagement.Core.Services.BasketService.Command;
using MenuManagement.Core.Services.BasketService.Command.AddUserInformationCommand;
using MenuManagement.Core.Services.BasketService.Query;
using MenuOrder.Shared.Controller;
using MenuOrder.Shared.Models;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MenuManagement.BasketMicroService.API.Controllers
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponse))]
        public async Task<bool> AddBasket([FromBody]JObject request)
        {
            return await Mediator.Send(new AddUserBasketCartInformationCommand { CartInformation = request , Username = _profileUser.Username });
        }

        [HttpPost("/api/basket/personalInfo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponse))]
        public async Task<bool> AddUserInformationInCart([FromBody] AddUserInformationInBasketCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponse))]
        public async Task<bool> UpdateBasket([FromBody] JObject request)
        {
            return await Mediator.Send(new RemoveUserCartItemCommand { CartInformation = request, Username = _profileUser.Username });
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponse))]
        public async Task<bool> DeleteBasket()
        {
            return await Mediator.Send(new DeleteUserCartItemCommand { Username = _profileUser.Username });
        }
    }
}
