using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace Inventory.Microservice.Core.Services.VendorMenus.Commands
{
    public class AddVendorMenusCommand : IRequest<VendorMenuDto>
    {
        public VendorMenuDto Menu { get; set; }
    }

    public class AddVendorMenusCommandHandler : IRequestHandler<AddVendorMenusCommand, VendorMenuDto>
    {
        private readonly IVendorsMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public AddVendorMenusCommandHandler(IVendorsMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<VendorMenuDto> Handle(AddVendorMenusCommand request, CancellationToken cancellationToken)
        {
            var mapToMenuModel = _mapper.Map<VendorsMenus>(request.Menu);
            var result = await _menuRepository.AddVendorMenus(mapToMenuModel);

            var mapToDto = _mapper.Map<VendorMenuDto>(result);
            return mapToDto;
        }
    }
}
