using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class AddApiResourceScopeCommand :  IRequest<ApiResourceScopeModel>
    {
        public int ScopeId { get; set; }
        public int ApiResourceId { get; set; }
    }

    public class AddApiResourceScopeCommandHandler : IRequestHandler<AddApiResourceScopeCommand, ApiResourceScopeModel>
    {
        private readonly IUtilsService _utilsService;

        public AddApiResourceScopeCommandHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        public async Task<ApiResourceScopeModel> Handle(AddApiResourceScopeCommand request, CancellationToken cancellationToken)
        {
            return await _utilsService.AddApiResourceScope(request.ScopeId,request.ApiResourceId);
        }
    }
}
