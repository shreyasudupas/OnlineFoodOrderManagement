using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class GetApiScopesViewModel : BaseStatusModel
    {
        public List<GetApiScopeModel> ApiScopes { get; set; }
    }

    public class GetApiScopeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
