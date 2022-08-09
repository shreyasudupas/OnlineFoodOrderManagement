using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;

namespace IdentityServer.API.GraphQL.Types.OutputTypes
{
    public class UserAddressType : ObjectType<UserProfileAddress>
    {
        protected override void Configure(IObjectTypeDescriptor<UserProfileAddress> descriptor)
        {
            descriptor.Field(_ => _.Id)
                .Type<LongType>()
                .Description("Address ID")
                .Name("id");

            descriptor.Field(_ => _.FullAddress)
                .Type<StringType>()
                .Description("Complete Address")
                .Name("fullAddress");

            descriptor.Field(_ => _.City)
                .Type<StringType>()
                .Description("City")
                .Name("city");

            descriptor.Field(_ => _.Area)
                .Type<StringType>()
                .Description("Area")
                .Name("area");

            descriptor.Field(_ => _.State)
                .Type<StringType>()
                .Description("State")
                .Name("state");

            descriptor.Field(_ => _.IsActive)
                .Type<BooleanType>()
                .Description("Address Active/Not Active")
                .Name("isActive");

            descriptor.Field(_ => _.MyStates)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users States List")
                .Name("myStates");

            descriptor.Field(_ => _.MyCities)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users Cities List")
                .Name("myCities");

            descriptor.Field(_ => _.MyAreas)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users MyAreas List")
                .Name("myAreas");
        }
    }
}
