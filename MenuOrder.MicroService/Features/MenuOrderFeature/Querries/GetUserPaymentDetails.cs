using MediatR;
using MenuOrder.MicroService.Features.MenuOrderFeature.Response;
using MenuOrder.MicroService.Models;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Querries
{
    public class GetUserPaymentDetails:IRequest<UserPaymentDetailsReponse>
    {
        public UserHeaderInfo UserInfo { get; set; }
        public string Token { get; set; }
    }
}
