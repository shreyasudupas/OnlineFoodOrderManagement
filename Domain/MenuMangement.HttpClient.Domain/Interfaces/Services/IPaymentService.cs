namespace MenuMangement.HttpClient.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<bool> PaymentByRewardPoints(string url, string userId, string token, string bodyContent);
}