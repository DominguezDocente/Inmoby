using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.Contracts.Repositories
{
    public interface ICitiesRepository : IRepository<City>
    {
        Task<PaginationResponse<City>> GetPagedListAsync(PaginationRequest request,
                                                         Guid? stateId,
                                                         string? filter,
                                                         CancellationToken cancellationToken = default);
    }
}
