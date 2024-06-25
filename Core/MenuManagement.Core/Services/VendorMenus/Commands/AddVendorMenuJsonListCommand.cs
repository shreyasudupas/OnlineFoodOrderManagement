using AutoMapper;
using Inventory.Microservice.Core.Common.SchemaGenerator;
using MediatR;
using MenuManagment.Mongo.Domain.Models;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.VendorMenus.Commands
{
    public class AddVendorMenuJsonListCommand : IRequest<bool>
    {
        public string Content { get; set; }
        public string VendorId { get; set; }
    }

    public class AddVendorMenuJsonListCommandHandler : IRequestHandler<AddVendorMenuJsonListCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IVendorsMenuRepository _vendorsMenuRepository;

        public AddVendorMenuJsonListCommandHandler(IMapper mapper,
            IVendorsMenuRepository vendorsMenuRepository)
        {
            _mapper = mapper;
            _vendorsMenuRepository = vendorsMenuRepository;
        }

        public async Task<bool> Handle(AddVendorMenuJsonListCommand request, CancellationToken cancellationToken)
        {
            List<VendorMenuDto> vendorMenus;
            try
            {
                vendorMenus = JsonSchmeaGeneratorFunction.CheckIfFileContentMatchesSchema<List<VendorMenuDto>>(request.Content);

            } catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            var result = await _vendorsMenuRepository.AddVendorMenuList(vendorMenus);
            return result;
        }

        public List<VendorMenuDto> CreateVendorMenuDtoList(List<AddVendorMenuJson> jsonDataList,string vendorId)
        {
            var vendorMenuDtoRequestList = new List<VendorMenuDto>();
            foreach (var json in jsonDataList)
            {
                vendorMenuDtoRequestList.Add(new VendorMenuDto()
                {
                    ItemName= json.ItemName,
                    Price= json.Price,
                    VendorId = vendorId,
                    Rating = 0,
                    FoodType= json.FoodType,
                    Discount= json.Discount,
                    Category= json.Category,
                    Active= json.Active,
                    Image = new MenuManagment.Mongo.Domain.Entities.SubModel.ImageModel()
                });
            }

            return vendorMenuDtoRequestList;
        }
    }
}
