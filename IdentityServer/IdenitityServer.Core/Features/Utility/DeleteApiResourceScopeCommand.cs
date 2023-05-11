using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class DeleteApiResourceScopeCommand : IRequest<bool>
    {
        public string ScopeName { get; set; }
        public int ApiResourceId { get; set; }
    }

    public class DeleteApiResourceScopeCommandHandler : IRequestHandler<DeleteApiResourceScopeCommand, bool>
    {
        private readonly IUtilsService _utilsService;

        public DeleteApiResourceScopeCommandHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        public async Task<bool> Handle(DeleteApiResourceScopeCommand request, CancellationToken cancellationToken)
        {
            return await _utilsService.DeleteApiResourceScope(request.ScopeName,request.ApiResourceId);
        }
    }
}
