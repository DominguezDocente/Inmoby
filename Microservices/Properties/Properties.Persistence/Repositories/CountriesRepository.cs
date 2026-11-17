using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class CountriesRepository : Repository<Country>, ICountriesRepository
    {
        private readonly DataContext _context;

        public CountriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<Country>> GetPagedListAsync(PaginationRequest request,
                                                                         string? filter,
                                                                         CancellationToken cancellationToken = default)
        {
            IQueryable<Country> query = _context.Set<Country>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string normalizedFilter = filter.Trim().ToLower();
                query = query.Where(country => country.Name.ToLower().Contains(normalizedFilter));
            }

            query = query.OrderBy(country => country.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
