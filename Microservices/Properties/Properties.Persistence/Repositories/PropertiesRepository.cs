using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using Properties.Persistence.Extensions;

namespace Properties.Persistence.Repositories
{
    public class PropertiesRepository : Repository<Property>, IPropertiesRepository
    {
        private readonly DataContext _context;

        public PropertiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        // TODO: Remove images from bucket
        public async Task<bool> DeleteWithImagesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Property? property = await _context.Set<Property>()
                                       .Include(p => p.Images)
                                       .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (property is null)
            {
                return false;
            }

            _context.RemoveRange(property.Images);
            _context.Remove(property);

            return true;
        }

        public async Task<Property?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Property>().Include(p => p.PropertyType)
                                                 .Include(p => p.Neighborhood)
                                                   .ThenInclude(n => n.City)
                                                 .Include(p => p.Amenities)
                                                 .Include(p => p.Images)
                                                 .AsSplitQuery()
                                                 .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PaginationResponse<Property>> GetPagedListAsync(PaginationRequest request,
                                                                              Guid? propertyTypeId,
                                                                              Guid? cityId,
                                                                              int? stratum,
                                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<Property> query = _context.Set<Property>()
                                                 .Include(p => p.PropertyType)
                                                 .Include(p => p.Neighborhood)
                                                    .ThenInclude(n => n.City)
                                                 .AsQueryable();

            if (propertyTypeId.HasValue)
            {
                query = query.Where(p => p.PropertyTypeId == propertyTypeId);
            }

            if (cityId.HasValue)
            {
                query = query.Where(p => p.Neighborhood!.CityId == cityId);
            }

            if (stratum.HasValue)
            {
                query = query.Where(p => p.Details.Stratum == stratum);
            }

            query = query.OrderByDescending(p => p.CreatedAt);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
