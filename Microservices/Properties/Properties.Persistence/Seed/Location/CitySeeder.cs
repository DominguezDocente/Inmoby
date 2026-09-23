using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Locations;

namespace Properties.Persistence.Seed.Location
{
    public sealed class CitySeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public CitySeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 3;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Cities.AnyAsync(cancellationToken))
            {
                return;
            }

            State antioquia = await _context.States
                .FirstAsync(s => s.Name == "Antioquia", cancellationToken);

            await _context.Cities.AddAsync(new City("Medellin", antioquia.Id), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
