using Properties.Application.Utilities.Pagination;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.Contracts.Repositories
{
    public interface ICountriesRepository : IRepository<Country>
    {
        Task<PaginationResponse<Country>> GetPagedListAsync(PaginationRequest request,
                                                            string? filter,
                                                            CancellationToken cancellationToken = default);
    }
}
