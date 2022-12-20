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
                .Name("id");

            descriptor.Field(_ => _.TwoFactorEnabled)
                .Type<BooleanType>()
                .Name("twoFactorEnabled");

            descriptor.Field(_ => _.PhoneNumberConfirmed)
                .Type<StringType>()
                .Name("phoneNumberConfirmed");

            descriptor.Field(_ => _.PhoneNumber)
                .Type<StringType>()
                .Description("Phone Number")
                .Name("phoneNumber");

            descriptor.Field(_ => _.ConcurrencyStamp)
                .Type<StringType>()
                .Name("concurrencyStamp");

            descriptor.Field(_ => _.SecurityStamp)
                .Type<StringType>()
                .Name("securityStamp");

            descriptor.Field(_ => _.EmailConfirmed)
                .Type<BooleanType>()
                .Description("Email Comfirmed")
                .Name("emailConfirmed");

            descriptor.Field(_ => _.NormalizedEmail)
                .Type<StringType>()
                .Description("Normalized Email")
                .Name("normalizedEmail");

            descriptor.Field(_ => _.Email)
                .Type<StringType>()
                .Description("Email")
                .Name("email");

            descriptor.Field(_ => _.NormalizedUserName)
                .Type<StringType>()
                .Description("Normalized UserName")
                .Name("normalizedUserName");

            descriptor.Field(_ => _.UserName)
                .Type<StringType>()
                .Description("UserName")
                .Name("userName");

            descriptor.Field(_ => _.LockoutEnabled)
                .Type<BooleanType>()
                .Name("lockoutEnabled");

            descriptor.Field(_ => _.AccessFailedCount)
                .Type<IntType>()
                .Name("accessFailedCount");

            descriptor.Field(_ => _.IsAdmin)
                .Type<BooleanType>()
                .Description("Is Admin")
                .Name("isAdmin");

            descriptor.Field(_ => _.ImagePath)
                .Type<StringType>()
                .Description("Image Path")
                .Name("imagePath");

            descriptor.Field(_ => _.CartAmount)
                .Type<IntType>()
                .Description("Cart Amount")
                .Name("cartAmount");

            descriptor.Field(_ => _.Points)
                .Type<DecimalType>()
                .Description("Points")
                .Name("points");

            descriptor.Field(_ => _.Address)
                .Type<ListType<UserAddressType>>()
                .Description("User Address Informations")
                .Name("address");

            descriptor.Field(_ => _.Claims)
                .Type<ListType<DropdownOutputType>>()
                .Description("User Claims")
                .Name("claims");

            descriptor.Field(_ => _.Roles)
                .Type<ListType<DropdownOutputType>>()
                .Description("User Roles")
                .Name("roles");

            descriptor.Field(_ => _.CreatedDate)
                .Type<DateTimeType>()
                .Description("User Created Date")
                .Name("createdDate");

        }
    }
}
