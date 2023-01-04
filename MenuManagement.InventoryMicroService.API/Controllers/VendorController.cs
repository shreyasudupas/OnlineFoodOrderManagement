using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Services.MenuInventoryService.AddCategory;
using MenuManagement.Core.Services.MenuInventoryService.AddVendorDetail;
using MenuManagement.Core.Services.MenuInventoryService.GetAllCategories;
using MenuManagement.Core.Services.MenuInventoryService.GetVendorById;
using MenuManagement.Core.Services.MenuInventoryService.Vendor.GetCategoryById;
using MenuManagement.Core.Services.MenuInventoryService.Vendor.UpdateVendorDetails;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Commands.AddVendors;
using MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorList;
using MenuOrder.Shared.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.InventoryMicroService.API.Controllers
{
    [Authorize]
    public class VendorController : BaseController
    {
        //[AllowAnonymous]
        [HttpGet("/api/vendors")]
        public async Task<List<VendorDto>> GetAllVendors()
        {
            return await Mediator.Send(new VendorListQuery());
        }

        [HttpGet("/api/vendor/{vendorId}")]
        public async Task<VendorDto> GetVendorById(string vendorId)
        {
            return await Mediator.Send(new GetVendorByIdQuery { VendorId = vendorId });
        }

        //[AllowAnonymous]
        [HttpPost("/api/vendors")]
        public async Task<List<VendorDto>> AddVendorsList([FromBody]AddVendorsCommand addVendorsCommand)
        {
            return await Mediator.Send(addVendorsCommand);
        }

        [HttpPost("/api/vendor")]
        public async Task<VendorDto> AddVendorDetail([FromBody] AddVendorDetailCommand addVendorDetail)
        {
            return await Mediator.Send(addVendorDetail);
        }

        [HttpPut("/api/vendor")]
        public async Task<VendorDto> UpdateVendorDetail([FromBody] UpdateVendorDetailsCommand updateVendorDetails)
        {
            return await Mediator.Send(updateVendorDetails);
        }

        [HttpGet("/api/vendor/categories/{vendorId}")]
        public async Task<List<CategoryDto>> GetAllVendorCategories(string vendorId)
        {
            return await Mediator.Send(new GetAllCategoriesQuery { VendorId = vendorId });
        }

        [HttpPost("/api/vendor/add/category")]
        public async Task<CategoryDto> AddVendorCategory([FromBody] AddCategoryCommand addCategoryCommand)
        {
            return await Mediator.Send(addCategoryCommand);
        }

        [HttpGet("/api/vendor/{vendorId}/category/{categoryId}")]
        public async Task<CategoryDto> GetAllVendorCategories(string vendorId,string categoryId)
        {
            return await Mediator.Send(new GetCategoryByIdQuery { VendorId = vendorId , Id = categoryId });
        }
    }
}
