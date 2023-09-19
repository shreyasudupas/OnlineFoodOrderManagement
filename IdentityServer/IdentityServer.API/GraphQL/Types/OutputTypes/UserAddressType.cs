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

            descriptor.Field(_ => _.CityId)
                .Type<StringType>()
                .Description("City ID")
                .Name("cityId");

            descriptor.Field(_ => _.Area)
                .Type<StringType>()
                .Description("Area")
                .Name("area");

             descriptor.Field(_ => _.AreaId)
                .Type<StringType>()
                .Description("Area Id")
                .Name("areaId");

            descriptor.Field(_ => _.State)
                .Type<StringType>()
                .Description("State")
                .Name("state");

            descriptor.Field(_ => _.StateId)
                .Type<StringType>()
                .Description("State Id")
                .Name("stateId");

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

            descriptor.Field(_ => _.VendorId)
                .Type<StringType>()
                .Description("Vendor Id")
                .Name("vendorId");

            descriptor.Field(_ => _.Editable)
                .Type<BooleanType>()
                .Description("Address Editable")
                .Name("editable");

            descriptor.Field(_ => _.Latitude)
                .Type<DecimalType>()
                .Description("Address Latitude")
                .Name("latitude");

            descriptor.Field(_ => _.Longitude)
                .Type<DecimalType>()
                .Description("Address Longitude")
                .Name("longitude");
        }
    }
}
