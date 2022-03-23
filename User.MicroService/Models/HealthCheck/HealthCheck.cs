namespace Identity.MicroService.Models.HealthCheck
{
    /// <summary>
    /// Describes each Health status of package or custom pacakage added
    /// </summary>
    public class HealthCheck
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
