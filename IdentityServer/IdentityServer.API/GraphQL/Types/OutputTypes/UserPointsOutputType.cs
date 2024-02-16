using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;

namespace IdentityServer.API.GraphQL.Types.OutputTypes
{
    public class UserPointsOutputType : ObjectType<UserPointsEvent>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPointsEvent> descriptor)
        {
            descriptor.Field(_ => _.EventId)
                .Type<LongType>()
                .Description("Event Id")
                .Name("eventId");

            descriptor.Field(_ => _.UserId)
                .Type<StringType>()
                .Description("User Id")
                .Name("userId");

            descriptor.Field(_ => _.PointsInHand)
                .Type<FloatType>()
                .Description("Points Available to this user which can be used")
                .Name("pointsInHand");

            descriptor.Field(_ => _.PointsAdjusted)
                .Type<FloatType>()
                .Description("Points Adjusted during add or spent or adjusted")
                .Name("pointsAdjusted");

            descriptor.Field(_ => _.EventOperation)
                .Type<StringType>()
                .Description("Event Type either be Adjusted/Added/Spent")
                .Name("eventOperation");

            descriptor.Field(_ => _.AddOrAdjustedUserId)
                .Type<StringType>()
                .Description("UserId if its Adjusted or System if Added")
                .Name("addOrAdjustedUserId");

            descriptor.Field(_ => _.EventCreatedDate)
                .Type<StringType>()
                .Description("Event Created Date")
                .Name("eventCreatedDate");
        }
    }
}
