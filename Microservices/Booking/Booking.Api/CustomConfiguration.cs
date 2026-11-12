using Booking.Api.Data;
using Booking.Api.Services.Abstractions;
using Booking.Api.Services.External.Abtractions;
using Booking.Api.Services.External.Implemetations;
using Booking.Api.Services.Implemetations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Booking.Api
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // DataContext configuration
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookingConnection"));
            });

            builder.Services.AddHttpContextAccessor();


            // Services
            AddServices(builder);

            // IAM: Identity adn Access Management
            AddIAM(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IBookingService, BookingService>();

            string? propertiesBaseUrl = builder.Configuration["Services:Properties:BaseUrl"];

            if (string.IsNullOrWhiteSpace(propertiesBaseUrl))
            {
                throw new InvalidOperationException("La configuración Services:Properties:BaseUrl es requerida.");
            }

            builder.Services.AddHttpClient<IPropertiesClient, PropertiesClient>(client =>
            {
                client.BaseAddress = new Uri(propertiesBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            IConfigurationSection jwt = builder.Configuration.GetSection("JWT");

            string? signinKey = jwt["SignKey"];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signinKey));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidIssuer = jwt["Issuer"],
                                    ValidateAudience = true,
                                    ValidAudience = jwt["Audience"],
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = key,
                                    ValidateLifetime = true,
                                    ClockSkew = TimeSpan.FromSeconds(30),
                                };
                            });

            builder.Services.AddAuthorization();

        }
    }
}
