using Common.Utility.Models;
using Common.Utility.Models.CartInformationModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Response
{
    public class UserPaymentDetailsReponse
    {
        public UserInfo UserInfo { get; set; }
        public int TotalAmount { get; set; }
        public int MyProperty { get; set; }
    }
}
