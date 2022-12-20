using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class ApiResourceMutationResolver
    {
        private readonly IAdministrationService _administrationService;

        public ApiResourceMutationResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<ApiResourceModel> AddApiResource(ApiResourceModel apiResourceModel)
        {
            return await _administrationService.AddApiResource(apiResourceModel);
        }

        public async Task<ApiResourceModel> DeleteApiResourceById(int id)
        {
            return await _administrationService.DeleteApiResourceById(id);
        }
    }
}
