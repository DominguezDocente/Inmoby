using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.Locations.Queries.GetCityById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetCitiesList
{
    public sealed class GetCitiesListQuery : IRequest<Result<PaginationResponse<CityDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public Guid? StateId { get; set; }
        public string? Filter { get; set; }
    }

    public sealed class GetCitiesListUseCase : BaseHandler, IRequestHandler<GetCitiesListQuery, Result<PaginationResponse<CityDTO>>>
    {
        private readonly ICitiesRepository _repository;

        public GetCitiesListUseCase(ICitiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<CityDTO>>> Handle(GetCitiesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationResponse<City> response = await _repository.GetPagedListAsync(query.Pagination,
                                                                                       query.StateId,
                                                                                       query.Filter);

                List<CityDTO> items = response.Items.Select(city => new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    StateId = city.StateId
                }).ToList();

                return PaginationResponse<CityDTO>.Create(items, response.TotalCount, query.Pagination);
            });
        }
    }
}
