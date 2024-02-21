using GraphQL;
using GraphQL.Client.Abstractions;
using MenuMangement.HttpClient.Domain.Exceptions;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using MenuMangement.HttpClient.Domain.Models.Graphql;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.SpendUserPointsGraphqlClient;

public class UserPointsMutationGraphqlClient : IUserPointsMuationGraphqlClient
{
    private readonly IGraphQLClient _client;
    private readonly ILogger<UserPointsMutationGraphqlClient> _logger;

    public UserPointsMutationGraphqlClient(IGraphQLClient client,
        ILogger<UserPointsMutationGraphqlClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string?> SpendUserPoints(string userId,double points)
    {
        var query = new GraphQLRequest
        {
            Query = @"mutation SpendUserPoints($userInfo:UserPointsAddSpendInput) {
  spendPoints(userPointsSpend: $userInfo) {
    result
  }
}",
            Variables = new { userInfo = new { UserId = userId, Points = points } }
        };

        try
        {
            var response = await _client.SendMutationAsync<SpendPointsResponseType>(query);

            if(response.Errors?.Length is null)
            {
                if (response.Data.SpendPoints.Result is not null)
                {
                    return response.Data.SpendPoints.Result;
                }
                else
                {
                    return null;
                }
            } 
            else
            {
                _logger.LogError(JsonSerializer.Serialize(response.Errors));
                return null;
            }
            
        } 
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message, ex);
        }
    }

    public async Task<string?> AdjustUserPoints(string userId,double points,string adjustedUserId)
    {
        var query = new GraphQLRequest
        {
            Query = @"mutation AdjustUserPoints($userInfo:UserPointsAdjustedInput) {
  userPointsAdjusted(userPointsAdjusted: $userInfo) {
    result
  }
}",
            Variables = new { userInfo = new { userId = userId, points = points, adjustedUserId = adjustedUserId } }
        };

        try
        {
            var response = await _client.SendMutationAsync<AdjustUserPointsResponseType>(query);

            if (response.Errors?.Length is null)
            {
                if (response.Data.UserPointsAdjusted.Result is not null)
                {
                    return response.Data.UserPointsAdjusted.Result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                _logger.LogError(JsonSerializer.Serialize(response.Errors));
                return null;
            }

        }
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message, ex);
        }
    }
}
