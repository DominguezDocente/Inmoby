using Microsoft.EntityFrameworkCore;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using Properties.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Repositories
{
    public class PropertiesRepository : Repository<Property>, IPropertiesRepository
    {
        private readonly DataContext _context;

        public PropertiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<Property?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Set<Property>()
                .Include(property => property.PropertyType)
                .Include(property => property.Neighborhood)
                    .ThenInclude(neighborhood => neighborhood.City)
                .Include(property => property.Amenities)
                .Include(property => property.Images)
                .AsSplitQuery()
                .SingleOrDefaultAsync(property => property.Id == id, cancellationToken);
        }

        public async Task<bool> DeleteWithImagesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Property? property = await _context.Set<Property>()
                .Include(item => item.Images)
                .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

            if (property is null)
            {
                return false;
            }

            _context.RemoveRange(property.Images);
            _context.Remove(property);
            return true;
        }

        public async Task<PaginationResponse<Property>> GetPagedListAsync(PaginationRequest request,
                                                                              Guid? propertyTypeId, 
                                                                              Guid? cityId, 
                                                                              int? stratum, 
                                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<Property> query = _context.Set<Property>()
                .AsNoTracking()
                .Include(property => property.PropertyType)
                .Include(property => property.Neighborhood)
                    .ThenInclude(neighborhood => neighborhood.City)
                .AsSplitQuery();

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
