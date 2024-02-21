namespace MenuMangement.HttpClient.Domain.Interfaces.GraphQl
{
    public interface IUserPointsGraphQlClient
    {
        Task<double?> GetRewardPointsFromUserId(string userId);
    }
}
