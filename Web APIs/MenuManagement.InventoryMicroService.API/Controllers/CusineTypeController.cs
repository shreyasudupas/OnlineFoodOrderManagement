using Inventory.Microservice.Core.Services.MenuInventoryService.CuisineTypes.Command;
using Inventory.Microservice.Core.Services.MenuInventoryService.CuisineTypes.Query;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class CusineTypeController : BaseController
    {
        [HttpGet("/api/cuisine/list")]
        public async Task<List<VendorCuisineDto>> GetAllCuisines([FromQuery]bool isActive)
        {
            return await Mediator.Send(new GetAllCuisineTypeQuery { Active = isActive });
        }

        [HttpGet("/api/cuisine/{cuisineId}")]
        public async Task<VendorCuisineDto> GetCuisineById(string cuisineId)
        {
            return await Mediator.Send(new GetCuisineTypeByIdQuery { Id = cuisineId });
        }

        [HttpPost("/api/cuisine")]
        public async Task<VendorCuisineDto> AddCuisine([FromBody] AddCuisineTypeCommand addCuisineTypeCommand)
        {
            return await Mediator.Send(addCuisineTypeCommand);
        }

        [HttpPut("/api/cuisine")]
        public async Task<VendorCuisineDto> UpdateCuisine([FromBody] UpdateCuisineTypeCommand updateCuisineTypeCommand)
        {
            return await Mediator.Send(updateCuisineTypeCommand);
        }
    }
}
