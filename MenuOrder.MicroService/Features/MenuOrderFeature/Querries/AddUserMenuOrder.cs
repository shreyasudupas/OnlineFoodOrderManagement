using MediatR;
using MenuOrder.MicroService.Features.MenuOrderFeature.Response;
using MenuOrder.MicroService.Models;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Querries
{
    public class AddUserMenuOrder:IRequest<MenuOrderResponse>
    {
        public string UserName { get; set; }
    }
}
