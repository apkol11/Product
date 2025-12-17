using Business.Interfaces.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Register Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IColourRepository, ColourRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();

            return services;
        }
    }
}
