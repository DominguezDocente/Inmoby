using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Properties.Persistence.Seed
{
    /// <summary>
    /// Ejecuta todos los <see cref="IDataSeeder"/> registrados, ordenados por <see cref="IDataSeeder.Order"/>.
    /// </summary>
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using IServiceScope scope = services.CreateScope();

            ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger(nameof(DatabaseSeeder));

            IEnumerable<IDataSeeder> seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(s => s.Order);

            foreach (IDataSeeder seeder in seeders)
            {
                logger.LogInformation("Ejecutando seeder {Seeder}", seeder.GetType().Name);
                await seeder.SeedAsync(cancellationToken);
            }
        }
    }
}
