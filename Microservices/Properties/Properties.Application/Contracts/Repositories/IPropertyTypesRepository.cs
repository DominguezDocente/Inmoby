using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.Contracts.Repositories
{
    public interface IPropertyTypesRepository : IRepository<PropertyType>
    {
        Task<PaginationResponse<PropertyType>> GetPagedListAsync(PaginationRequest request,
                                                                  string? filter,
                                                                  CancellationToken cancellationToken = default);
    }
}
