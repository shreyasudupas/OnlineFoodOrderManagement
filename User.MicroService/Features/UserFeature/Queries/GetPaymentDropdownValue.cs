using Identity.MicroService.Data;
using MediatR;
using System.Collections.Generic;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetPaymentDropdownValue:IRequest<List<PaymentDropDown>>
    {
    }
}
