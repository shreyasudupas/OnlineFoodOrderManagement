using HotChocolate.Types;
using IdenitityServer.API.Types.OutputTypes;
using IdenitityServer.Core.QueryResolvers;
using System.Threading;

namespace IdentityServer.API.GraphQL.Query
{
    
    public class UserInformationExtensionType : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");

            descriptor.Field("getUserInfo")
                .Description("Gets all User Information by providing UserId")
                .Argument("UserId", _ => _.Type<StringType>())
                .Resolve(context =>
                {
                    CancellationToken ct = context.RequestAborted;
                    var UserId = context.ArgumentValue<string>("UserId");

                    GetUserInformationResolver service = context.Service<GetUserInformationResolver>();
                    return service.GetUserInfo(UserId);
                })
                .Type<UserInformationOutputType>()
                .Name("UserInformation");

        }
    }
}
