using HotChocolate.Types;
using IdenitityServer.Core.Domain.Response;

namespace IdentityServer.API.GraphQL.Types.OutputTypes
{
    public class SaveUserInformationOutputType : ObjectType<SaveUserResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<SaveUserResponse> descriptor)
        {
            descriptor
                .Field(_=>_.Result)
                .Type<BooleanType>()
                .Description("User ID")
                .Name("result");
                
        }
    }
}
