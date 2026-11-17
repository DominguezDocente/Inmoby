using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class PropertyTypesRepository : Repository<PropertyType>, IPropertyTypesRepository
    {
        private readonly DataContext _context;

        public PropertyTypesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<PropertyType>> GetPagedListAsync(PaginationRequest request,
                                                                              string? filter,
                                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<PropertyType> query = _context.Set<PropertyType>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string normalizedFilter = filter.Trim().ToLower();
                query = query.Where(propertyType => propertyType.Name.ToLower().Contains(normalizedFilter));
            }

            query = query.OrderBy(propertyType => propertyType.Name);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
