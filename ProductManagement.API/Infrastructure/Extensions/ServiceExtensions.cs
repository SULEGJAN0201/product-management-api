using ProductManagment.Core.Services;
using ProductManagment.Infrastructure.Data;
using ProductManagment.Services.Services;

namespace ProductManagement.API.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for registering services in the dependency injection container
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Register all application services
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Services
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
