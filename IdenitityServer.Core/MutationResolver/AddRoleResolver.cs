using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class AddRoleResolver
    {
        private readonly IAdministrationService _administrationService;

        public AddRoleResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<RoleListResponse> AddRole(RoleListResponse newRole)
        {
            return await _administrationService.AddRole(newRole);
        }
    }
}
