using System.Collections.Generic;

namespace MicroService.Shared.Models
{
    public class UserCartInfo
    {
        public UserInfo UserInfo { get; set; }
        public List<CartItems> Items { get; set; }
    }
}
