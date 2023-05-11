using System;

namespace IdenitityServer.Core.Domain.Model
{
    public class ClientBasicInfo
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int AccessTokenLifetime { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public bool RequireClientSecret { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
