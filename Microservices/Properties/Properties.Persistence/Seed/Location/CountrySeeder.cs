using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Locations;

namespace Properties.Persistence.Seed.Location
{
    public sealed class CountrySeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public CountrySeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Countries.AnyAsync(cancellationToken))
            {
                return;
            }

            await _context.Countries.AddAsync(new Country("Colombia"), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
