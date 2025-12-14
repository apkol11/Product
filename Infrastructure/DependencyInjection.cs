using Infrastructure.Data;
using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here

            services.AddDbContext<ApplicationDbContext>(options =>

             options.UseSqlite("Data Source=app.db")
            );

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IColourRepository, ColourRepository>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();

            return services;
        }
    }
}
