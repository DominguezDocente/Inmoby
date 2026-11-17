using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class CitiesRepository : Repository<City>, ICitiesRepository
    {
        private readonly DataContext _context;

        public CitiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<City>> GetPagedListAsync(PaginationRequest request,
                                                                      Guid? stateId,
                                                                      string? filter,
                                                                      CancellationToken cancellationToken = default)
        {
            IQueryable<City> query = _context.Set<City>().AsNoTracking();

            if (stateId.HasValue)
            {
                query = query.Where(city => city.StateId == stateId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string normalizedFilter = filter.Trim().ToLower();
                query = query.Where(city => city.Name.ToLower().Contains(normalizedFilter));
            }

            query = query.OrderBy(city => city.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
