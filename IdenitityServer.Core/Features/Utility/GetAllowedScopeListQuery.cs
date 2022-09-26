using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAllowedScopeListQuery : IRequest<List<DropdownModel>>
    {
    }

    public class GetAllowedScopeListQueryHandler : IRequestHandler<GetAllowedScopeListQuery, List<DropdownModel>>
    {
        private readonly IUtilsService _utilsService;
        public GetAllowedScopeListQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }
        public Task<List<DropdownModel>> Handle(GetAllowedScopeListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_utilsService.GetAllowedScopeList());
        }
    }
}
