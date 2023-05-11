using Identity.MicroService.Models.APIResponse;
using MediatR;
using MenuInventory.Microservice.Features.VendorFeature.Querries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Common.Mongo.Database.Data.Context;

namespace MenuInventory.Microservice.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        //private readonly IVendorBL _vendor;

        //public VendorController(IVendorBL vendor)
        //{
        //    _vendor = vendor;
        //}
        private readonly IMediator _mediator;
        private readonly MenuRepository _menuRepository;

        public VendorController(IMediator mediator, MenuRepository menuRepository)
        {
            _mediator = mediator;
            _menuRepository = menuRepository;
        }

        [HttpGet]
        public string GetVendor()
        {
            return "This is vendor Controller";
        }

        /// <summary>
        /// This is to get the Vendor List
        /// </summary>
        /// <returns>List of string</returns>
        /// <response code="404">No Vendors</response>
        /// <response code="500">Exception in code</response>
        [HttpGet]
        public async Task<Response> GetVendorListAsync()
        {
            var getResult = await _mediator.Send(new GetAllVendorRequest());
            if (getResult.Count > 0)
            {
                return new Response(HttpStatusCode.OK, getResult, null);
            }
            else
            {
                return new Response(HttpStatusCode.NotFound, null, null);
            }
        }
    }
}
