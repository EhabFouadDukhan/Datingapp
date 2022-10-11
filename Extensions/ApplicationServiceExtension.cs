using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIII.Data;
using APIIII.Helpers;
using APIIII.Interfaces;
using APIIII.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APIIII.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
        
    }
}