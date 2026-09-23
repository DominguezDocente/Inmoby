using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Locations;

namespace Properties.Persistence.Seed.Location
{
    public sealed class NeighborhoodSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public NeighborhoodSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 4;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Neighborhoods.AnyAsync(cancellationToken))
            {
                return;
            }

            City medellin = await _context.Cities
                .FirstAsync(c => c.Name == "Medellin", cancellationToken);

            List<Neighborhood> neighborhoods =
            [
                new Neighborhood("El Poblado", medellin.Id),
                new Neighborhood("Laureles", medellin.Id),
                new Neighborhood("Belén", medellin.Id),
                new Neighborhood("Robledo", medellin.Id),
                new Neighborhood("Buenos Aires", medellin.Id),
            ];

            await _context.Neighborhoods.AddRangeAsync(neighborhoods, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
