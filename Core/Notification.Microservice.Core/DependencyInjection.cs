using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notification.Microservice.Core.Mapping;
using System.Reflection;

namespace Notification.Microservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //mapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new NotificationProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
