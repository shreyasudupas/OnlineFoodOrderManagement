namespace MenuMangement.HttpClient.Domain.Models.Graphql
{
    public class GetRewardResponseType
    {
        public UserInformation UserInformation { get; set; }
    }

    public class UserInformation
    {
        public double Points { get; set; }
    }
}
