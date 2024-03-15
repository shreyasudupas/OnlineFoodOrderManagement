using FluentValidation;
using MenuManagment.Mongo.Domain.Enum;
using OrderManagement.Microservice.Core.Querries.Orders.GetVendorOrders;

namespace OrderManagement.Microservice.Core.Validators;

public class GetVendorOrdersBasedOnOrderStatusQueryValidator : AbstractValidator<GetVendorOrdersBasedOnOrderStatusQuery>
{
    public GetVendorOrdersBasedOnOrderStatusQueryValidator() 
    {
        RuleFor(order=>order.VendorId).NotNull().NotEmpty();
        RuleFor(order => order.VendorStatus).Must(CheckIfValidStatus).WithMessage("Vendor Status passed is not valid");
    }

    private bool CheckIfValidStatus(string[] status)
    {
        var success = true;
        foreach (var statusItem in status)
        {
            OrderStatusEnum orderStatus;
            var isSuccess  = Enum.TryParse(statusItem,out orderStatus);
            if(isSuccess == false)
            {
                return isSuccess;
            }
        }

        return success;
    }
}
