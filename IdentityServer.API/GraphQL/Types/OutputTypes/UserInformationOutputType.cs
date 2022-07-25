using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.API.GraphQL.Types.OutputTypes;

namespace IdenitityServer.API.Types.OutputTypes
{
    public class UserInformationOutputType : ObjectType<UserProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<UserProfile> descriptor)
        {
            descriptor.Field(_ => _.Id)
                .Type<StringType>()
                .Description("User ID")
                .Name("Id");

            descriptor.Field(_ => _.TwoFactorEnabled)
                .Type<BooleanType>()
                .Name("TwoFactorEnabled");

            descriptor.Field(_ => _.PhoneNumberConfirmed)
                .Type<StringType>()
                .Name("PhoneNumberConfirmed");

            descriptor.Field(_ => _.PhoneNumber)
                .Type<StringType>()
                .Description("Phone Number")
                .Name("PhoneNumber");

            descriptor.Field(_ => _.ConcurrencyStamp)
                .Type<StringType>()
                .Name("ConcurrencyStamp");

            descriptor.Field(_ => _.SecurityStamp)
                .Type<StringType>()
                .Name("SecurityStamp");

            descriptor.Field(_ => _.EmailConfirmed)
                .Type<BooleanType>()
                .Description("Email Comfirmed")
                .Name("EmailConfirmed");

            descriptor.Field(_ => _.NormalizedEmail)
                .Type<StringType>()
                .Description("Normalized Email")
                .Name("NormalizedEmail");

            descriptor.Field(_ => _.Email)
                .Type<StringType>()
                .Description("Email")
                .Name("Email");

            descriptor.Field(_ => _.NormalizedUserName)
                .Type<StringType>()
                .Description("Normalized UserName")
                .Name("NormalizedUserName");

            descriptor.Field(_ => _.UserName)
                .Type<StringType>()
                .Description("UserName")
                .Name("UserName");

            descriptor.Field(_ => _.LockoutEnabled)
                .Type<BooleanType>()
                .Name("LockoutEnabled");

            descriptor.Field(_ => _.AccessFailedCount)
                .Type<IntType>()
                .Name("AccessFailedCount");

            descriptor.Field(_ => _.IsAdmin)
                .Type<BooleanType>()
                .Description("Is Admin")
                .Name("IsAdmin");

            descriptor.Field(_ => _.ImagePath)
                .Type<StringType>()
                .Description("Image Path")
                .Name("ImagePath");

            descriptor.Field(_ => _.CartAmount)
                .Type<IntType>()
                .Description("Cart Amount")
                .Name("CartAmount");

            descriptor.Field(_ => _.Points)
                .Type<DecimalType>()
                .Description("Points")
                .Name("Points");

            descriptor.Field(_ => _.Address)
                .Type<ListType<UserAddressType>>()
                .Description("User Address Informations")
                .Name("Address");

            descriptor.Field(_ => _.Claims)
                .Type<AnyType>()
                .Description("User Claims")
                .Name("Claims");

            descriptor.Field(_ => _.Roles)
                .Type<AnyType>()
                .Description("User Roles")
                .Name("Roles");

        }
    }
}
