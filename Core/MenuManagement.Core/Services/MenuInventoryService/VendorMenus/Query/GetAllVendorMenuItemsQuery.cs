using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class GetAllVendorMenuItemsQuery : IRequest<List<VendorMenuDto>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorMenuItemsQueryHandler : IRequestHandler<GetAllVendorMenuItemsQuery, List<VendorMenuDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllVendorMenuItemsQueryHandler(IVendorsMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorMenuDto>> Handle(GetAllVendorMenuItemsQuery request, CancellationToken cancellationToken)
        {
            var result = await _menuRepository.GetAllVendorMenuByVendorId(request.VendorId);
            if (result.Count > 0)
            {
                var mapToModel = _mapper.Map<List<VendorMenuDto>>(result);
                return mapToModel;
            }
            else
                return null;
        }
    }
}
