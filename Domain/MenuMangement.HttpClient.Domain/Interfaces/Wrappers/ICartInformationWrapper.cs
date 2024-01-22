namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers
{
    public interface ICartInformationWrapper
    {
        Task<bool> ClearCartInformation(string userId,string token);
    }
}
