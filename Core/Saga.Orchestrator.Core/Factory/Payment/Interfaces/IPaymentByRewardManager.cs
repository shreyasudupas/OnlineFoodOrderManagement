namespace Saga.Orchestrator.Core.Factory.Payment.Interfaces
{
    public interface IPaymentByRewardManager
    {
        Task<double?> GetRewardFromUser(string userId);
    }
}