using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.Contracts.Repositories
{
    public interface IPropertiesRepository : IRepository<Property>
    {
        Task<(List<Property> items, int totalCount)> GetPagedListAsync(PaginationRequest request,
                                                                       Guid? PropertyTypeId,
                                                                       Guid? cityId,
                                                                       string? Stratum,
                                                                       CancellationToken cancellationToken = default);
    }
}
