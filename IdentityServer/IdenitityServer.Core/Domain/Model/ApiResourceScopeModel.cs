namespace IdenitityServer.Core.Domain.Model
{
    public class ApiResourceScopeModel
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ApiResourceId { get; set; }
    }
}
