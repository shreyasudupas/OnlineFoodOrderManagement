namespace IdenitityServer.Core.Domain.Request
{
    public class UserPointsAdjustedInput
    {
        public string UserId { get; set; }
        public double Points { get; set; }
        public string AdminId { get; set; }
    }
}
