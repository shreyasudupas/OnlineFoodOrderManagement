using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class ApiScopeQueryResolver
    {
        private readonly IAdministrationService _administrationService;

        public ApiScopeQueryResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<List<ApiScopeModel>> GetAllApiScopes()
        {
            return await _administrationService.GetApiScopes();
        }

        public async Task<ApiScopeModel> GetApiScopeById(int Id)
        {
            return await _administrationService.GetApiScopeById(Id);
        }
    }
}
