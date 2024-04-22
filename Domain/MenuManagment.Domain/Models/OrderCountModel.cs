namespace MenuManagment.Mongo.Domain.Models
{
    public class OrderCountModel
    {
        public int OrderPlacedCount { get; set; }
        public int OrderInProgressCount { get; set; }
        public int OrderCancelledCount { get; set; }
    }
}
