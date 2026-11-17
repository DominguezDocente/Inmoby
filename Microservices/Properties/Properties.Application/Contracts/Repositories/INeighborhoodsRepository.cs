using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.Contracts.Repositories
{
    public interface INeighborhoodsRepository : IRepository<Neighborhood>
    {
        Task<PaginationResponse<Neighborhood>> GetPagedListAsync(PaginationRequest request,
                                                                 Guid? cityId,
                                                                 string? filter,
                                                                 CancellationToken cancellationToken = default);
    }
}
