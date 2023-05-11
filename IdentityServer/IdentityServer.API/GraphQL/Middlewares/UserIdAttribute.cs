using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System.Reflection;

namespace IdentityServer.API.GraphQL.Middlewares
{
    public class UserIdAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use<UserIdMiddleware>();
        }
    }
}
