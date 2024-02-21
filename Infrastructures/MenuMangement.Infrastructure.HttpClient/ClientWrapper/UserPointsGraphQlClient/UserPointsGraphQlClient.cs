using GraphQL;
using GraphQL.Client.Abstractions;
using MenuMangement.HttpClient.Domain.Exceptions;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using MenuMangement.HttpClient.Domain.Models.Graphql;
using Microsoft.Extensions.Logging;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.UserPointsGraphQlClient
{
    public class UserPointsGraphQlClient : IUserPointsGraphQlClient
    {
        private readonly IGraphQLClient _client;
        private readonly ILogger _logger;

        public UserPointsGraphQlClient(IGraphQLClient client,
            ILogger<UserPointsGraphQlClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<double?> GetRewardPointsFromUserId(string userId)
        {
            var query = new GraphQLRequest
            {
                Query = @"query GetCurrentUserEvent($userId:String) {
  currentUserPointsEvent(userId:$userId) {
    pointsInHand
  }
}",
                Variables = new { userId = userId }
            };

            try
            {
                var response = await _client.SendQueryAsync<CurrentUserPointsEventResponseType>(query);
                if (response.Data.CurrentUserPointsEvent is not null)
                {
                    return response.Data.CurrentUserPointsEvent.PointsInHand;
                }
                else
                {
                    return null;
                }
            } catch(Exception ex)
            {
                _logger.LogError("Get Current User Points Event encountred Error in graphQl {0}",ex.Message);
                throw new GraphQLException("Get Current User Points Event encountred Error", ex);
            }
        }
    }
}
