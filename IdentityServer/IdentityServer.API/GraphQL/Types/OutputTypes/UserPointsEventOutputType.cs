using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;

namespace IdentityServer.API.GraphQL.Types.OutputTypes
{
    public class UserPointsEventOutputType : ObjectType<UserPointResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPointResponse> descriptor)
        {
            descriptor.Field(_=>_.Result)
                .Type<StringType>()
                .Description("Event Added or Failure Result")
                .Name("result");
        }
    }
}
