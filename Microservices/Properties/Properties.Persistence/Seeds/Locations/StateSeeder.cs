using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Locations;

namespace Properties.Persistence.Seeds.Locations
{
    public class StateSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public StateSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.States.AnyAsync(cancellationToken))
            {
                return;
            }

            Country colombia = await _context.Countries.FirstAsync(c => c.Name == "Colombia");

            await _context.States.AddAsync(new State("Antioquia", colombia.Id), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
