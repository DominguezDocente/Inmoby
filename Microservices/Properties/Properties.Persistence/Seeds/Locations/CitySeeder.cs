using Properties.Domain.Entities.Locations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Persistence.Seeds.Locations
{
    public class CitySeeder : IDataSeeder
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

            State antioquia = await _context.States.FirstAsync(s => s.Name == "Antioquia");

            await _context.Cities.AddAsync(new City("Medellín", antioquia.Id), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
