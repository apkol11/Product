using AutoMapper;
using Business.Handler;
using Business.Interfcae.Handler;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register your handlers
            services.AddTransient<IProductHandler, ProductHandler>();

            // Register AutoMapper with your MappingProfile
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<MappingProfile>();
            });

            return services;
        }
    }
}
