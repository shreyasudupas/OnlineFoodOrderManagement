using AutoMapper;
using FluentValidation;
using MediatR;
using MenuManagment.Mongo.Domain.MappingProfiles.OrderManagement;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OrderManagement.Microservice.Core
{
    public static class DependacyInjection
    {
        public static void OrderManagementCore(this IServiceCollection serviceCollection)
        {
            //mapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CartInformationProfile());
                mc.AddProfile(new OrderInformationProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);

            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}