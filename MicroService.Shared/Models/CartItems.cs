namespace Common.Utility.Models
{
    public class CartItems
    {
        public long id {get;set;}
        public string menuItem { get; set; }
        public int price { get; set; }
        public long vendorId { get; set; }
        public string vendorName { get; set; }
        public string menuType { get; set; }
        public string imagePath { get; set; }
        public string offerPrice { get; set; }
        public string createdDate { get; set; }
        public int quantity { get; set; }
    }
}
