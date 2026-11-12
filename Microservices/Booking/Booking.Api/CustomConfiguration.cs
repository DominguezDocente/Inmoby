using Booking.Api.Data;
using Booking.Api.Data.Seeders;
using Booking.Api.Services.Abstractions;
using Booking.Api.Services.External.Abstractions;
using Booking.Api.Services.External.Implementations;
using Booking.Api.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Booking.Api
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookingConnection"));
            });

            builder.Services.AddScoped<IBookingsService, BookingsService>();
            builder.Services.AddScoped<SeedDb>();

            builder.Services.AddHttpContextAccessor();

            string? propertiesBaseUrl = builder.Configuration["Services:Properties:BaseUrl"];

            if (string.IsNullOrWhiteSpace(propertiesBaseUrl))
            {
                throw new InvalidOperationException("La configuración Services:Properties:BaseUrl es requerida.");
            }

            builder.Services.AddHttpClient<IPropertiesClient, PropertiesClient>(client =>
            {
                client.BaseAddress = new Uri(propertiesBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(5);
            });

            return builder;
        }

        public static async Task AddCustomWebApplicationConfigurationAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            SeedDb seeder = scope.ServiceProvider.GetRequiredService<SeedDb>();
            await seeder.SeedAsync();
        }
    }
}
