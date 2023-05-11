using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.VendorIdMapping
{
    public class AddVendorIdMappingCommand : IRequest<VendorIdMappingResponse>
    {
        public VendorIdMappingResponse VendorIdAdd { get; set; }
    }

    public class AddVendorIdMappingCommandHandler : IRequestHandler<AddVendorIdMappingCommand, VendorIdMappingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AddVendorIdMappingCommandHandler(IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<VendorIdMappingResponse> Handle(AddVendorIdMappingCommand request, CancellationToken cancellationToken)
        {
            var mapDtoToModel = _mapper.Map<VendorUserIdMapping>(request.VendorIdAdd);
            var result = await _userService.AddVendorUserIdMapping(mapDtoToModel);
            var dtoToModel = _mapper.Map<VendorIdMappingResponse>(result);

            return dtoToModel;
        }
    }
}
