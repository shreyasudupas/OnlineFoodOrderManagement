using MenuManagment.Mongo.Domain.Interfaces.Repository.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;
using OrderManagement.Mongo.Persistance.Repositories;

namespace OrderManagement.Mongo.Persistance
{
    public static class DependencyInjection
    {
        public static void AddOrderManagementPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICartInformationRepository, CartInformationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}