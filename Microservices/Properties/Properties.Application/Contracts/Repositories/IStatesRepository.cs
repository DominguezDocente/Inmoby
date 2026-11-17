using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.Contracts.Repositories
{
    public interface IStatesRepository : IRepository<State>
    {
        Task<PaginationResponse<State>> GetPagedListAsync(PaginationRequest request,
                                                          Guid? countryId,
                                                          string? filter,
                                                          CancellationToken cancellationToken = default);
    }
}
