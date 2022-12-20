using Identity.MicroService.Models.APIResponse;
using MediatR;
using MenuInventory.Microservice.Features.VendorCartConfigurationFeature.Querries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class VendorCartConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorCartConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Response> GetVendorCartConfigruation(string VendorId)
        {
            var result = await _mediator.Send(new GetVendorCartConfig(VendorId));
            if(result != null)
            {
                return new Response(HttpStatusCode.OK, result, null);
            }
            else
            {
                return new Response(HttpStatusCode.NotFound, null, null);
            }
        }
    }
}
