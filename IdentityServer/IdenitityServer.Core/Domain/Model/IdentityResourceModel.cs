namespace IdenitityServer.Core.Domain.Model
{
    public class IdentityResourceModel
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public bool Required { get; set; }
        public bool Empahasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public string Created { get; set; }
        public string? Updated { get; set; }
        public bool NonEditable { get; set; }
    }
}
