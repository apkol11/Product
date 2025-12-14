using Business.Handlers;
using Business.Interfaces.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IProductHandler, ProductHandler>();
            services.AddTransient<IColourHandler, ColourHandler>();
            services.AddTransient<IProductTypeHandler, ProductTypeHandler>();

            return services;
        }
    }
}
