namespace Saga.Orchestrator.Core.Factory.Payment.Interfaces;

public interface IPaymentByRewardManager
{
    Task<double?> GetRewardFromUser(string userId);
    Task<string?> SpendUserPoints(string userId, double points);
    Task<string?> AdjustUserPoints(string userId, double points, string adjustedUserId);
}