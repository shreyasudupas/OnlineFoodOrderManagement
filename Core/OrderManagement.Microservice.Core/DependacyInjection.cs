﻿using AutoMapper;
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
            });

            IMapper mapper = mapperConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);

            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}