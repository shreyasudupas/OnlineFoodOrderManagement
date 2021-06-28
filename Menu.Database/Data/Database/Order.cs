using System;

namespace OrderAPI.Models
{
    public class Order
    {
        public Guid CustomerId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
