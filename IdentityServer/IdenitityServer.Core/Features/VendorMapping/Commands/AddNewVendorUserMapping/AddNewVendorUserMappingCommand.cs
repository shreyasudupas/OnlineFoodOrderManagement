using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Commands.AddNewVendorUserMapping;

public class AddNewVendorUserMappingCommand : IRequest<Unit>
{
    public VendorIdMappingResponse UserVendorIdMapping { get; set; }
}

public class AddNewVendorUserMappingCommandHandler : IRequestHandler<AddNewVendorUserMappingCommand,Unit>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AddNewVendorUserMappingCommandHandler(
        IMapper mapper,
        IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<Unit> Handle(AddNewVendorUserMappingCommand request, CancellationToken cancellationToken)
    {
        var mapToVendorUserMapping = _mapper.Map<VendorUserIdMapping>(request.UserVendorIdMapping);
        await _userService.AddNewVendorUserIdMappingAsync(mapToVendorUserMapping,cancellationToken);
        return Unit.Value;
    }
}