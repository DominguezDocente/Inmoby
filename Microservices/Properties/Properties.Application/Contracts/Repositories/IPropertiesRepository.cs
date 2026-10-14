using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.Contracts.Repositories
{
    public interface IPropertiesRepository : IRepository<Property>
    {
        Task<Property?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteWithImagesAsync(Guid id, CancellationToken cancellationToken = default);

        Task<PaginationResponse<Property>> GetPagedListAsync(PaginationRequest request,
                                                                       Guid? PropertyTypeId,
                                                                       Guid? cityId,
                                                                       int? Stratum,
                                                                       CancellationToken cancellationToken = default);
    }
}
