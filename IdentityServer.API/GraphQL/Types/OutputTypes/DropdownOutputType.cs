using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;

namespace IdentityServer.API.GraphQL.Types.OutputTypes
{
    public class DropdownOutputType : ObjectType<DropdownModel>
    {
        protected override void Configure(IObjectTypeDescriptor<DropdownModel> descriptor)
        {
            descriptor.Field(_ => _.Label)
                .Type<StringType>()
                .Description("Label")
                .Name("Label");

            descriptor.Field(_ => _.Value)
                .Type<IntType>()
                .Description("Value")
                .Name("Value");
        }
    }
}
