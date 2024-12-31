using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace Inventory.Microservice.Core.Services.VendorMenus.Query
{
    public class GetAllVendorMenuItemsQuery : IRequest<List<VendorMenuDto>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorMenuItemsQueryHandler : IRequestHandler<GetAllVendorMenuItemsQuery, List<VendorMenuDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;

        public GetAllVendorMenuItemsQueryHandler(IVendorsMenuRepository menuRepository,
            IMapper mapper,
            IVendorRepository vendorRepository)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _vendorRepository = vendorRepository;
        }

        public async Task<List<VendorMenuDto>> Handle(GetAllVendorMenuItemsQuery request, CancellationToken cancellationToken)
        {
            var vendorDetail = await _vendorRepository.GetVendorDocument(request.VendorId);
            if(vendorDetail is null)
            {
                return new();
            }

            var vendorCategories = vendorDetail.Categories;
            var allCategoriesMenuList = await _menuRepository.GetAllVendorMenuByVendorId(request.VendorId);
            var allCategoriesMenuListDto = VendorMenuDto.MaptoDto(allCategoriesMenuList, vendorCategories);

            var currentDate = DateTime.UtcNow;

            var activeCategoriesIds = vendorDetail.Categories
                .Where(x=>x.CategoryReleaseDate <= currentDate && x.Active is true)
                .Select(x=>x.Id)
                .ToList();

            var activeCategoryList = allCategoriesMenuListDto
                .Where(x => activeCategoriesIds.Contains(x.CategoryDetails.CategoryId))
                .Select(x=>x)
                .ToList();

            return activeCategoryList;
        }

        public record GroupByCategoryMenuList(string CategoryId,List<VendorMenuDto> VendorMenus);

        
    }
}
