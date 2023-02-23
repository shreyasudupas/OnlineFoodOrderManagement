﻿using Inventory.Microservice.Core.Common.Services;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MenuOrder.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedInjection(this IServiceCollection services)
        {
            services.AddSingleton<IProfileUser, ProfileUser>();
            return services;
        }
    }
}
