using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace Saga.Orchestrator.Core.Factory.Payment
{
    public interface IPaymentFactory
    {
        Task<double?> GetPaymentBasedOnUserSelection(string userId, PaymentDetailDto paymentDetail);
        Task<string?> SpendUserPayment(string userId, string paymentMethod, double price);
        Task<string?> AdjustUserPayment(PaymentInformationRecord paymentInformation, string adjustedUserId);
    }
}