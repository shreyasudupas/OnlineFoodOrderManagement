using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Command
{
    public class GetAllVendorMenusQuery : IRequest<List<VendorMenuDto>>
    {
    }

    public class GetAllVendorMenusQueryHandler : IRequestHandler<GetAllVendorMenusQuery, List<VendorMenuDto>>
    {
        private readonly IVendorsMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllVendorMenusQueryHandler(IVendorsMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorMenuDto>> Handle(GetAllVendorMenusQuery request, CancellationToken cancellationToken)
        {
            var result = await _menuRepository.GetAllMenu();
            if (result.Count > 0)
            {
                var mapToDto = _mapper.Map<List<VendorMenuDto>>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}
