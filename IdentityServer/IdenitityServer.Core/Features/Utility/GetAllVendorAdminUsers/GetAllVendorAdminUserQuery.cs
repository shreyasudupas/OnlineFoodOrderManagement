using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.GetAllVendorAdminUsers
{
    public class GetAllVendorAdminUserQuery : IRequest<IEnumerable<VendorAdminUserResponseModel>>
    {
    }

    public class GetAllVendorAdminUserQueryHandler : IRequestHandler<GetAllVendorAdminUserQuery, IEnumerable<VendorAdminUserResponseModel>>
    {
        private readonly IUtilsService _utilsService;

        public GetAllVendorAdminUserQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        public async Task<IEnumerable<VendorAdminUserResponseModel>> Handle(GetAllVendorAdminUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _utilsService.GetAllVendorAdminUsers();
            return result;
        }
    }
}
