using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Common.Utility.Models
{
    public class UserCartInfo
    {
        public UserInfo UserInfo { get; set; }
        public List<CartItems> Items { get; set; }
    }
}
