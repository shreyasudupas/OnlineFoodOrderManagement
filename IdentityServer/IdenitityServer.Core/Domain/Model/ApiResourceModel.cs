using System;
using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.Model
{
    public class ApiResourceModel
    {
        public ApiResourceModel()
        {
            Scopes = new List<ApiResourceScopeModel>();
        }
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? DisplayName{ get; set; }
        public string? AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public string? Created { get; set; }
        public string? Updated { get; set; }
        public string? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
        public List<ApiResourceScopeModel>? Scopes { get; set; }
    }
}
