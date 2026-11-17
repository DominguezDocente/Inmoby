using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.Locations.Queries.GetCountryById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetCountriesList
{
    public sealed class GetCountriesListQuery : IRequest<Result<PaginationResponse<CountryDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public string? Filter { get; set; }
    }

    public sealed class GetCountriesListUseCase : BaseHandler, IRequestHandler<GetCountriesListQuery, Result<PaginationResponse<CountryDTO>>>
    {
        private readonly ICountriesRepository _repository;

        public GetCountriesListUseCase(ICountriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<CountryDTO>>> Handle(GetCountriesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationResponse<Country> response = await _repository.GetPagedListAsync(query.Pagination,
                                                                                           query.Filter);

                List<CountryDTO> items = response.Items.Select(country => new CountryDTO
                {
                    Id = country.Id,
                    Name = country.Name
                }).ToList();

                return PaginationResponse<CountryDTO>.Create(items, response.TotalCount, query.Pagination);
            });
        }
    }
}
