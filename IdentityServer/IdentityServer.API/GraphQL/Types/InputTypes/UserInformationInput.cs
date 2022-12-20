using HotChocolate.Types;
using IdenitityServer.Core.Domain.Request;

namespace IdentityServer.API.GraphQL.Types.InputTypes
{
    public class UserInformationInput : InputObjectType<UserInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserInput> descriptor)
        {
            descriptor.Field(_ => _.Id)
                .Type<StringType>()
                .Description("User ID")
                .Name("id");

            descriptor.Field(_ => _.UserName)
                .Type<StringType>()
                .Description("UserName")
                .Name("userName");

            descriptor.Field(_ => _.Email)
                .Type<StringType>()
                .Description("Email")
                .Name("email");

            descriptor.Field(_ => _.IsAdmin)
                .Type<BooleanType>()
                .Description("Is Admin")
                .Name("isAdmin");

            descriptor.Field(_ => _.CartAmount)
                .Type<IntType>()
                .Description("Cart Amount")
                .Name("cartAmount");

            descriptor.Field(_ => _.Points)
                .Type<DecimalType>()
                .Description("Points")
                .Name("points");
        }
    }
}
