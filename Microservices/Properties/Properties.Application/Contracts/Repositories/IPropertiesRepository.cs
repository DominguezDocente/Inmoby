using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.Contracts.Repositories
{
    public interface IPropertiesRepository : IRepository<Property>
    {
        Task<PaginationResponse<Property>> GetPagedListAsync(PaginationRequest request,
                                                                       Guid? PropertyTypeId,
                                                                       Guid? cityId,
                                                                       int? Stratum,
                                                                       CancellationToken cancellationToken = default);
    }
}
