using HotChocolate.Types;
using IdenitityServer.Core.QueryResolvers;
using IdenitityServer.Core.Types.OutputTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    public record UserInfo(int id);
    public class UserInformationExtensionType : ObjectTypeExtension<UserInfo>
    {
        
        protected override void Configure(IObjectTypeDescriptor<UserInfo> descriptor)
        {

            descriptor.Field("getUserInformation")
                .ResolveWith<GetUserInformationResolver>(_ => _.GetUserInfo(default))
                .Type<UserInformationOutputType>()
                .Name("GetUserInformation");

        }
    }
}
