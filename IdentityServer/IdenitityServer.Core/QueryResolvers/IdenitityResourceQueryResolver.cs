using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class IdenitityResourceQueryResolver
    {
        private readonly IAdministrationService adminstrationService;

        public IdenitityResourceQueryResolver(IAdministrationService adminstrationService)
        {
            this.adminstrationService = adminstrationService;
        }

        public async Task<List<IdentityResourceModel>> GetAllIdentityResource()
        {
            return await adminstrationService.GetAllIdentityResource();
        }

        public async Task<IdentityResourceModel> GetIdentityResourceById(int Id)
        {
            return await adminstrationService.GetIdentityResourceById(Id);
        }
    }
}
