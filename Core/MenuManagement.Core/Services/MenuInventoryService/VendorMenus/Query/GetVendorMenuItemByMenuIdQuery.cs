﻿using AutoMapper;
using MediatR;
using Inventory.Microservice.Core.Common.Models.InventoryService.Response;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Query
{
    public class GetVendorMenuItemByMenuIdQuery : IRequest<VendorMenuResponse>
    {
        public string MenuId { get; set; }
    }

    public class GetVendorMenuItemByMenuIdQueryHandler : IRequestHandler<GetVendorMenuItemByMenuIdQuery, VendorMenuResponse>
    {
        private readonly IVendorsMenuRepository vendorsMenuRepository;
        private readonly IMenuImagesRepository menuImagesRepository;
        private readonly IMapper _mapper;

        public GetVendorMenuItemByMenuIdQueryHandler(IVendorsMenuRepository vendorsMenuRepository,
            IMenuImagesRepository menuImagesRepository, IMapper mapper)
        {
            this.vendorsMenuRepository = vendorsMenuRepository;
            this.menuImagesRepository = menuImagesRepository;
            _mapper = mapper;
        }

        public async Task<VendorMenuResponse> Handle(GetVendorMenuItemByMenuIdQuery request, CancellationToken cancellationToken)
        {
            var vendorMenu = await vendorsMenuRepository.GetVendorMenusByMenuId(request.MenuId);

            if(!string.IsNullOrEmpty(vendorMenu.ImageId))
            {
                var imageMenu = await menuImagesRepository.GetMenuImagesById(vendorMenu.ImageId);
                return new VendorMenuResponse
                {
                    Id = vendorMenu.Id,
                    VendorId = vendorMenu.VendorId,
                    Category = vendorMenu.Category,
                    FoodType = vendorMenu.FoodType,
                    ImageId = vendorMenu.ImageId,
                    ItemName = vendorMenu.ItemName,
                    ImageData = imageMenu.FileName, //send File name later once the file is found then replace with base64 image string
                    Price = vendorMenu.Price,
                    Active = vendorMenu.Active,
                    Discount = vendorMenu.Discount,
                    Rating = vendorMenu.Rating
                };
            }
            else
                return new VendorMenuResponse
                {
                    Id = vendorMenu.Id,
                    VendorId = vendorMenu.VendorId,
                    Category = vendorMenu.Category,
                    FoodType = vendorMenu.FoodType,
                    ItemName = vendorMenu.ItemName,
                    ImageId = "",
                    ImageData = "", 
                    Price = vendorMenu.Price,
                    Active = vendorMenu.Active,
                    Discount = vendorMenu.Discount,
                    Rating = vendorMenu.Rating
                };
        }
    }
}
