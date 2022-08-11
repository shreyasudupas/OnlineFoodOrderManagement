using HotChocolate.Types;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.MutationResolver;
using IdentityServer.API.GraphQL.Types.InputTypes;
using System.Threading;

namespace IdentityServer.API.GraphQL.Mutation
{
    public class AddUserInformationExtensionType : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");

            descriptor.Field("saveUserInfo")
                .Description("Save User Information")
                .Argument("user", _ => _.Type<UserInformationInput>())
                .Resolve(context =>
                {
                    CancellationToken ct = context.RequestAborted;
                    var user = context.ArgumentValue<UserInput>("user");

                    UpdateUserInfoResolver service = context.Service<UpdateUserInfoResolver>();
                    return service.AddUserInformation(new IdenitityServer.Core.Domain.DBModel.UserProfile 
                    { 
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsAdmin = user.IsAdmin,
                        CartAmount = user.CartAmount,
                        Points = user.Points
                    });
                })
                .Type<BooleanType>()
                .Name("modifyUserInformation");
        }
    }
}
