using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class ApiResourceQueryResolver
    {
        private readonly IAdministrationService _administrationService;

        public ApiResourceQueryResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<List<ApiResourceModel>> GetAllApiResources()
        {
            return await _administrationService.GetAllApiResources();
        }

        public async Task<ApiResourceModel> GetApiResourceById(int id)
        {
            return await _administrationService.GetApiResourceById(id);
        }
    }
}
