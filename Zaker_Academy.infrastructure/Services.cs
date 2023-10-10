using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Repository;

namespace Zaker_Academy.infrastructure
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register your custom services here
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Replace with your actual service registration
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            return services;
        }
    }
}