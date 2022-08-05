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
                .Name("Id");

            descriptor.Field(_ => _.FullAddress)
                .Type<StringType>()
                .Description("Complete Address")
                .Name("FullAddress");

            descriptor.Field(_ => _.City)
                .Type<StringType>()
                .Description("City")
                .Name("City");

            descriptor.Field(_ => _.Area)
                .Type<StringType>()
                .Description("Area")
                .Name("Area");

            descriptor.Field(_ => _.State)
                .Type<StringType>()
                .Description("State")
                .Name("State");

            descriptor.Field(_ => _.IsActive)
                .Type<BooleanType>()
                .Description("Address Active/Not Active")
                .Name("IsActive");

            descriptor.Field(_ => _.MyStates)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users States List")
                .Name("MyStates");

            descriptor.Field(_ => _.MyCities)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users Cities List")
                .Name("MyCities");

            descriptor.Field(_ => _.MyAreas)
                .Type<ListType<DropdownOutputType>>()
                .Description("Users MyAreas List")
                .Name("MyAreas");
        }
    }
}
