namespace MenuMangement.HttpClient.Domain.Models.Graphql;

public class SpendPointsResponseType
{
    public UserPointsResult SpendPoints { get; set; }
}

public class UserPointsResult
{
    public string Result { get; set; }
}
