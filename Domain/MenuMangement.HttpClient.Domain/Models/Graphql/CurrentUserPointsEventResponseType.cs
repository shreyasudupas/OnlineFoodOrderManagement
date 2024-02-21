namespace MenuMangement.HttpClient.Domain.Models.Graphql
{
    public class CurrentUserPointsEventResponseType
    {
        public CurrentUserPointsEvent CurrentUserPointsEvent { get; set; }
    }

    public class CurrentUserPointsEvent
    {
        public double PointsInHand { get; set; }
    }
}
