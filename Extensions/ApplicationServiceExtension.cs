using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIII.Data;
using APIIII.Interfaces;
using APIIII.Services;
using Microsoft.EntityFrameworkCore;

namespace APIIII.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
        
    }
}