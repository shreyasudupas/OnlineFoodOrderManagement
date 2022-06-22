using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdenitityServer.Core
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
