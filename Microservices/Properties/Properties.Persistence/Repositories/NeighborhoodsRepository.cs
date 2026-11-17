using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class NeighborhoodsRepository : Repository<Neighborhood>, INeighborhoodsRepository
    {
        private readonly DataContext _context;

        public NeighborhoodsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<Neighborhood>> GetPagedListAsync(PaginationRequest request,
                                                                              Guid? cityId,
                                                                              string? filter,
                                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<Neighborhood> query = _context.Set<Neighborhood>().AsNoTracking();

            if (cityId.HasValue)
            {
                query = query.Where(neighborhood => neighborhood.CityId == cityId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string normalizedFilter = filter.Trim().ToLower();
                query = query.Where(neighborhood => neighborhood.Name.ToLower().Contains(normalizedFilter));
            }

            query = query.OrderBy(neighborhood => neighborhood.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
