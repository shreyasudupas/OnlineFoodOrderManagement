namespace MenuMangement.HttpClient.Domain.Interfaces.GraphQl
{
    public interface IUserPointsMuationGraphqlClient
    {
        Task<string?> SpendUserPoints(string userId, double points);
        Task<string?> AdjustUserPoints(string userId, double points, string adjustedUserId);
    }
}
