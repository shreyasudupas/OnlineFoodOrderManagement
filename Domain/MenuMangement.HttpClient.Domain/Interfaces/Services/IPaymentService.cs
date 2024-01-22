using MenuManagment.Mongo.Domain.Dtos.OrderManagement;

namespace MenuMangement.HttpClient.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<bool> PaymentByRewardPoints(string userId, string token, PaymentDetailDto payementDetail);
}