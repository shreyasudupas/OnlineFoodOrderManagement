using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.MenuInventoryService.VendorMenus.Commands
{
    public class UpdateVendorMenuCommand : IRequest<VendorMenuDto>
    {
        public VendorMenuDto UpdateVendorMenu { get; set; }
    }

    public class UpdateVendorMenuCommandHandler : IRequestHandler<UpdateVendorMenuCommand, VendorMenuDto>
    {
        private readonly IVendorsMenuRepository vendorsMenuRepository;
        private readonly IMapper _mapper;

        public UpdateVendorMenuCommandHandler(IVendorsMenuRepository vendorsMenuRepository, IMapper mapper)
        {
            this.vendorsMenuRepository = vendorsMenuRepository;
            _mapper = mapper;
        }

        public async Task<VendorMenuDto> Handle(UpdateVendorMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await vendorsMenuRepository.UpdateVendorMenus(request.UpdateVendorMenu);
            if (result != null)
            {
                var mapToModel = _mapper.Map<VendorMenuDto>(result);
                return mapToModel;
            }
            else
                return null;
        }
    }
}
