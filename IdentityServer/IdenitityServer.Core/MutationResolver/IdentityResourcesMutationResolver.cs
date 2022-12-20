using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class IdentityResourcesMutationResolver
    {
        private readonly IAdministrationService adminstrationService;

        public IdentityResourcesMutationResolver(IAdministrationService adminstrationService)
        {
            this.adminstrationService = adminstrationService;
        }

        public async Task<IdentityResourceModel> AddIdentityResource(IdentityResourceModel identityResourceModel)
        {
            return await adminstrationService.AddIdentityResource(identityResourceModel);
        }

        public async Task<IdentityResourceModel> UpdateIdentityResource(IdentityResourceModel identityResourceModel)
        {
            return await adminstrationService.UpdateIdentityResource(identityResourceModel);
        }

        public async Task<bool> DeleteIdentityResource(int id)
        {
            return await adminstrationService.DeleteIdentityResource(id);
        }
    }
}
