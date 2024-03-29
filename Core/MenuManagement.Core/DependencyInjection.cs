﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Inventory.Microservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInventoryCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
