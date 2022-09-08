using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class MutationRoleResolver
    {
        private readonly IAdministrationService _administrationService;

        public MutationRoleResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public async Task<RoleListResponse> AddRole(RoleListResponse newRole)
        {
            return await _administrationService.AddRole(newRole);
        }

        public async Task<bool> DeleteRole(string roleId)
        {
            return await _administrationService.DeleteRole(roleId);
        }
    }
}
