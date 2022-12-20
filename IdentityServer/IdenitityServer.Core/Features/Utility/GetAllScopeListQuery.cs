using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class GetAllScopeListQuery : IRequest<List<DropdownModel>>
    {
    }

    public class GetAllScopeListQueryHandler : IRequestHandler<GetAllScopeListQuery, List<DropdownModel>>
    {
        private readonly IUtilsService _utilsService;
        public GetAllScopeListQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }
        public Task<List<DropdownModel>> Handle(GetAllScopeListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_utilsService.GetAllScopeList());
        }
    }
}
