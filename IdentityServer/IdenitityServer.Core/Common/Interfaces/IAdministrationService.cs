using IdenitityServer.Core.Domain.Model;
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

        Task<List<ApiScopeModel>> GetApiScopes();
        Task<ApiScopeModel> GetApiScopeById(int Id);
        Task<ApiScopeModel> AddApiScope(ApiScopeModel apiScope);
        Task<ApiScopeModel> SaveApiScope(ApiScopeModel apiScope);
        Task<ApiScopeModel> DeleteApiScope(int apiScopeId);
        Task<List<ApiResourceModel>> GetAllApiResources();
        Task<ApiResourceModel> GetApiResourceById(int id);
        Task<ApiResourceModel> AddApiResource(ApiResourceModel apiResourceModel);
        Task<ApiResourceModel> DeleteApiResourceById(int id);
        Task<List<IdentityResourceModel>> GetAllIdentityResource();
        Task<IdentityResourceModel> AddIdentityResource(IdentityResourceModel identityResourceModel);
        Task<IdentityResourceModel> UpdateIdentityResource(IdentityResourceModel identityResourceModel);
        Task<bool> DeleteIdentityResource(int id);
        Task<IdentityResourceModel> GetIdentityResourceById(int Id);
    }
}
