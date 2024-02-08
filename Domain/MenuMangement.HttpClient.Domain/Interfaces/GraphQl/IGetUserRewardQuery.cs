namespace MenuMangement.HttpClient.Domain.Interfaces.GraphQl
{
    public interface IGetUserRewardQuery
    {
        Task<double?> GetRewardFromUserAsync(string userId);
    }
}
