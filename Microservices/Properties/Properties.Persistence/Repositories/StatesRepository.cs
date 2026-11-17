using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class StatesRepository : Repository<State>, IStatesRepository
    {
        private readonly DataContext _context;

        public StatesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<State>> GetPagedListAsync(PaginationRequest request,
                                                                       Guid? countryId,
                                                                       string? filter,
                                                                       CancellationToken cancellationToken = default)
        {
            IQueryable<State> query = _context.Set<State>().AsNoTracking();

            if (countryId.HasValue)
            {
                query = query.Where(state => state.CountryId == countryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string normalizedFilter = filter.Trim().ToLower();
                query = query.Where(state => state.Name.ToLower().Contains(normalizedFilter));
            }

            query = query.OrderBy(state => state.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
