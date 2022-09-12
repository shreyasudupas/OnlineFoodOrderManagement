using IdenitityServer.Core.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IAdministrationService
    {
        List<RoleListResponse> Roles();
        Task<RoleListResponse> AddRole(RoleListResponse role);
        RoleListResponse GetRoleById(string RoleId);
        Task<RoleListResponse> DeleteRole(string roleId);

        RoleListResponse EditRoleById(RoleListResponse role);
    }
}
