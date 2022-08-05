using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.Features.Utility;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.APIControllers
{
    public class UtilitiesController : BaseController
    {
        [HttpGet("/api/utility/getCityByStateId")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<DropdownModel>))]
        public async Task<List<DropdownModel>> GetCityById(int StateId)
        {
            return await Mediator.Send(new GetCityByStateIdQuery { StateId = StateId });
        }

        [HttpGet("/api/utility/getAreaByCityId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
        public async Task<List<DropdownModel>> GetAreaById(int CityId)
        {
            return await Mediator.Send(new GetAreaByCityIdQuery { CityId = CityId });
        }
    }
}
