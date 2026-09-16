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

        public async Task<PaginationResponse<Property>> GetPagedListAsync(PaginationRequest request,
                                                                              Guid? propertyTypeId, 
                                                                              Guid? cityId, 
                                                                              int? stratum, 
                                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<Property> query = _context.Set<Property>()
                                                 .Include(p => p.PropertyTypeId)
                                                 .AsQueryable();

            if (propertyTypeId.HasValue)
            {
                query = query.Where(p => p.PropertyTypeId == propertyTypeId);
            }

            if (cityId.HasValue)
            {
                query = query.Where(p => p.Address.CityId == cityId);
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
