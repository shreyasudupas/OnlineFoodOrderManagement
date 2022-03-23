using Identity.MicroService.Data;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class GetDropDownHandler : IRequestHandler<GetPaymentDropdownValue, List<PaymentDropDown>>
    {
        private readonly UserContext userContext;

        public GetDropDownHandler(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<List<PaymentDropDown>> Handle(GetPaymentDropdownValue request, CancellationToken cancellationToken)
        {
            var GetDropdownValues = await userContext.PaymentDropDown.ToListAsync();
            return GetDropdownValues;
        }
    }
}
