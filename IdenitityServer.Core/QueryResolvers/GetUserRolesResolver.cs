using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using System.Collections.Generic;

namespace IdenitityServer.Core.QueryResolvers
{
    public class GetUserRolesResolver
    {
        private readonly IAdministrationService _administrationService;

        public GetUserRolesResolver(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public List<RoleListResponse> GetRoles()
        {
            var result = _administrationService.Roles();
            return result;
        }

        public RoleListResponse GetRoleById(string RoleId)
        {
            return _administrationService.GetRoleById(RoleId);
        }
    }
}
