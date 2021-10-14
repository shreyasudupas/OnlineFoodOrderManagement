using Common.Utility.Models;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Response
{
    public class UserPaymentDetailsReponse
    {
        public UserInfo UserInfo { get; set; }
        public int TotalAmount { get; set; }
    }
}
