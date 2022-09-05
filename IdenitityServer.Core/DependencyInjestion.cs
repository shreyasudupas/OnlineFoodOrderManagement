using IdenitityServer.Core.MutationResolver;
using IdenitityServer.Core.QueryResolvers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdenitityServer.Core
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<GetUserInformationResolver>();
            services.AddScoped<UpdateUserInfoResolver>();
            services.AddScoped<AddModifyUserAddressResolver>();
            services.AddScoped<GetUserListResolver>();

            return services;
        }
    }
}
