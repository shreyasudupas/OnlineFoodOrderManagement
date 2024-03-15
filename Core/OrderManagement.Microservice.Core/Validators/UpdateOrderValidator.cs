using FluentValidation;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Enum;

namespace OrderManagement.Microservice.Core.Validators;

public class UpdateOrderValidator : AbstractValidator<OrderInformationDto>
{
    public UpdateOrderValidator()
    {
        RuleFor(order => order.Id).NotEmpty().NotNull();

        RuleFor(order=>order.CartId).NotEmpty().NotNull();

        RuleFor(order => order).NotNull()
                               .NotEmpty()
                               .Must(StatusMustmatchCurrentStatus)
                               .WithMessage("Order status does not match/ status has incomplete information");
    }

    private bool StatusMustmatchCurrentStatus(OrderInformationDto order)
    {
        var status = true;
        var currentStatus = order.CurrentOrderStatus;

        if(string.IsNullOrEmpty(currentStatus))
        {
            return false;
        } else
        if (string.IsNullOrEmpty(order.Status.OrderPlaced) && 
            currentStatus.Equals(OrderStatusEnum.OrderPlaced.ToString(),StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        } else if (string.IsNullOrEmpty(order.Status.OrderInProgress) &&
            currentStatus.Equals(OrderStatusEnum.OrderInProgress.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        } else if (string.IsNullOrEmpty(order.Status.OrderReady) &&
            currentStatus.Equals(OrderStatusEnum.OrderReady.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        } else if (string.IsNullOrEmpty(order.Status.OrderDone) &&
            currentStatus.Equals(OrderStatusEnum.OrderDone.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        } else if (string.IsNullOrEmpty(order.Status.OrderCancelled) &&
            currentStatus.Equals(OrderStatusEnum.OrderCancelled.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }
        return status;
    }
}
