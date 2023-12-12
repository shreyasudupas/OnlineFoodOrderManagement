namespace Saga.Orchestrator.Core.Interfaces.Wrappers
{
    public interface ICartInformationWrapper
    {
        Task<bool> ClearCartInformation(string userId,string token);
    }
}
