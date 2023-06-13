using Inventory.Microservice.Core.Services.FoodTypes.Command;
using Inventory.Microservice.Core.Services.FoodTypes.Query;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class FoodTypeController : BaseController
    {
        [HttpGet("/api/foodtype/list")]
        public async Task<List<VendorFoodTypeDto>> GetAllFoodTypes([FromQuery] bool isActive)
        {
            return await Mediator.Send(new GetAllFoodTypeQuery { Active = isActive });
        }

        [HttpGet("/api/foodtype/{foodTypeId}")]
        public async Task<VendorFoodTypeDto> GetFoodTypeById(string foodTypeId)
        {
            return await Mediator.Send(new GetFoodTypeByIdQuery { Id = foodTypeId });
        }

        [HttpPost("/api/foodtype")]
        public async Task<VendorFoodTypeDto> AddFoodType([FromBody] AddFoodTypeCommand addFoodTypeCommand)
        {
            return await Mediator.Send(addFoodTypeCommand);
        }

        [HttpPut("/api/foodtype")]
        public async Task<VendorFoodTypeDto> UpdateFoodType([FromBody] UpdateFoodTypeCommand updateFoodTypeCommand)
        {
            return await Mediator.Send(updateFoodTypeCommand);
        }
    }
}
