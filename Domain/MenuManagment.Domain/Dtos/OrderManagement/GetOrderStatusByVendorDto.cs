namespace MenuManagment.Mongo.Domain.Dtos.OrderManagement
{
    public class GetOrderStatusByVendorDto
    {
        public string VendorId { get; set; }
        public string[] VendorStatus { get; set; }
    }
}
