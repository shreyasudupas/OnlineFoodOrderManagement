namespace IdenitityServer.Core.Domain.DBModel
{
    public class LocationArea
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int? CityId { get; set; }
    }
}
