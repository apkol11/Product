using Business.Handlers;
using Business.Interfaces.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register Handlers
            services.AddScoped<IProductHandler, ProductHandler>();
            services.AddScoped<IColourHandler, ColourHandler>();
            services.AddScoped<IProductTypeHandler, ProductTypeHandler>();

            return services;
        }
    }
}
