using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MicroService.Shared.Models.CartInformationModels
{
    public class UserCartInformation
    {
        public UserInfo UserInfo { get; set; }
        public List<JObject> Items { get; set; }
    }
}
