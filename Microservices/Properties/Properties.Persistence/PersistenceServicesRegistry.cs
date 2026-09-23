using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Persistence.Repositories;
using Properties.Persistence.Seed;
using Properties.Persistence.Seed.Location;
using Properties.Persistence.Seed.Properties;
using Properties.Persistence.UnitOfWorks;

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

            services.AddScoped<IDataSeeder, CountrySeeder>();
            services.AddScoped<IDataSeeder, StateSeeder>();
            services.AddScoped<IDataSeeder, CitySeeder>();
            services.AddScoped<IDataSeeder, NeighborhoodSeeder>();
            services.AddScoped<IDataSeeder, PropertyTypeSeeder>();

            return services;
        }
    }
}
