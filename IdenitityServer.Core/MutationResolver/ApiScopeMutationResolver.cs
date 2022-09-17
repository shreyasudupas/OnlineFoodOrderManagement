using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class ApiScopeMutationResolver
    {
        private readonly IAdministrationService _administrationService;
        public ApiScopeMutationResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<ApiScopeModel> AddApiScope(ApiScopeModel apiScope)
        {
            return await _administrationService.AddApiScope(apiScope);
        }

        public async Task<ApiScopeModel> SaveApiScope(ApiScopeModel apiScope)
        {
            return await _administrationService.SaveApiScope(apiScope); 
        }

        public async Task<ApiScopeModel> RemoveApiScope(int apiScopeId)
        {
            return await _administrationService.DeleteApiScope(apiScopeId);
        }
    }
}
