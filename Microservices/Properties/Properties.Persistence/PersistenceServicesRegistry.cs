using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Persistence.Repositories;
using Properties.Persistence.Seeds;
using Properties.Persistence.Seeds.Locations;
using Properties.Persistence.Seeds.Properties;
using Properties.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyConnection"));
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IPropertiesRepository, PropertiesRepository>();

            // Seeders
            services.AddScoped<IDataSeeder, CountrySeeder>();
            services.AddScoped<IDataSeeder, StateSeeder>();
            services.AddScoped<IDataSeeder, CitySeeder>();
            services.AddScoped<IDataSeeder, NeighborhoodSeeder>();
            services.AddScoped<IDataSeeder, PropertyTypeSeeder>();

            return services;
        }
    }
}
