namespace IdenitityServer.Core.Domain.Model
{
    public class CategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public bool Active { get; set; }
    }
}
