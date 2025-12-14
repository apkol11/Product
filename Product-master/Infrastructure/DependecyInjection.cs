using Business.Interfcae.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here

            services.AddDbContext<ApplicationDbContext>(options => 
             
             options.UseSqlite("Data Source=app.db")
            );

            services.AddTransient<IProductRepository, ProductRepository>();

            return services;
        }

    }
}
