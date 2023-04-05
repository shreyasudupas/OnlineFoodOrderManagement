using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.VendorMapping.Query.GetAllVendorUserMapping
{
    public class GetAllVendorUserMappingQuery : IRequest<List<VendorMappingResponse>>
    {
        public string VendorId { get; set; }
    }

    public class GetAllVendorUserMappingQueryHandler : IRequestHandler<GetAllVendorUserMappingQuery, List<VendorMappingResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IVendorUserMappingService _vendorUserMappingService;
        private readonly IUserService _userService;

        public GetAllVendorUserMappingQueryHandler(IMapper mapper,
            IVendorUserMappingService vendorUserMappingService,
            IUserService userService)
        {
            _mapper = mapper;
            _vendorUserMappingService = vendorUserMappingService;
            _userService = userService;
        }

        public async Task<List<VendorMappingResponse>> Handle(GetAllVendorUserMappingQuery request, CancellationToken cancellationToken)
        {
            var userMappingResult = await _vendorUserMappingService.GetVendorUserMapping(request.VendorId);
            var mapToResposne = _mapper.Map<List<VendorMappingResponse>>(userMappingResult);

            foreach(var user in mapToResposne)
            {
                if(user.UserId != null)
                {
                    var userInfo = await _userService.GetUserInformationById(user.UserId);
                    user.Username = userInfo.UserName;
                    user.EmailId = userInfo.Email;
                }
            }
            return mapToResposne;
        }
    }
}
