using HotChocolate.Types;
using IdenitityServer.Core.Domain.Request;
using IdenitityServer.Core.MutationResolver;
using IdentityServer.API.GraphQL.Types.InputTypes;
using IdentityServer.API.GraphQL.Types.OutputTypes;
using System.Threading;

namespace IdentityServer.API.GraphQL.Mutation
{
    public class SaveUserInformationExtensionType : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");

            descriptor.Field("saveUserInfo")
                .Description("Save User Information")
                .Argument("userInfoInput", _ => _.Type<UserInformationInput>())
                .Resolve(context =>
                {
                    CancellationToken ct = context.RequestAborted;
                    var user = context.ArgumentValue<UserInput>("userInfoInput");

                    UpdateUserInfoResolver service = context.Service<UpdateUserInfoResolver>();
                    return service.AddUserInformation(new IdenitityServer.Core.Domain.DBModel.UserProfile 
                    { 
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        //IsAdmin = user.IsAdmin,
                        UserType = user.UserType,
                        CartAmount = user.CartAmount,
                        Points = user.Points,
                        Enabled = user.Enabled,
                        EmailConfirmed = user.EmailConfirmed??false,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed??false,
                        Fullname = user.FullName
                    });
                })
                .Type<SaveUserInformationOutputType>()
                .Name("modifyUserInformation");
        }
    }
}
