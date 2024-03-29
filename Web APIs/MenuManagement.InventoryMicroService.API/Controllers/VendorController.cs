﻿using Inventory.Microservice.Core.Services.MenuInventoryService.AddCategory;
using Inventory.Microservice.Core.Services.MenuInventoryService.AddVendorDetail;
using Inventory.Microservice.Core.Services.MenuInventoryService.GetAllCategories;
using Inventory.Microservice.Core.Services.MenuInventoryService.GetVendorById;
using Inventory.Microservice.Core.Services.MenuInventoryService.Vendor.Commands;
using Inventory.Microservice.Core.Services.MenuInventoryService.Vendor.GetCategoryById;
using Inventory.Microservice.Core.Services.MenuInventoryService.Vendor.UpdateVendorDetails;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorDetails.Commands.AddVendors;
using Inventory.Microservice.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorList;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
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

        [HttpPut("/api/vendor/update/category")]
        public async Task<CategoryDto> UpdateVendorCategoryDetail([FromBody] UpdateCategoryItemCommand updateCategoryItemCommand)
        {
            return await Mediator.Send(updateCategoryItemCommand);
        }
    }
}
