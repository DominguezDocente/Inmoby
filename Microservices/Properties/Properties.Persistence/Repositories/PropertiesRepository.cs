using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
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

        public Task<(List<Property> items, int totalCount)> GetPagedListAsync(PaginationRequest request,
                                                                              Guid? PropertyTypeId, 
                                                                              Guid? cityId, 
                                                                              string? Stratum, 
                                                                              CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
