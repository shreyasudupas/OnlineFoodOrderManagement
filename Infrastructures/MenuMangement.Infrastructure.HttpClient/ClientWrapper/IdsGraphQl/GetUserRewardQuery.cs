using GraphQL;
using GraphQL.Client.Abstractions;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using MenuMangement.HttpClient.Domain.Models;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdsGraphQl
{
    public class GetUserRewardQuery : IGetUserRewardQuery
    {
        private readonly IGraphQLClient _client;

        public GetUserRewardQuery(IGraphQLClient client)
        {
            _client = client;
        }

        public async Task<double?> GetRewardFromUserAsync(string userId)
        {
            var query = new GraphQLRequest
            {
                Query = @"query GetCurrentUserPoint($userId:String){
  userInformation(userId: $userId) {
    points
  }
}",
                Variables = new { userId = userId } 
            };
            var response = await _client.SendQueryAsync<GetRewardResponseType>(query);

            if( response.Data.UserInformation is not null)
            {
                return response.Data.UserInformation.Points;
            }
            else
            {
                return null;
            }
        }
    }
}
