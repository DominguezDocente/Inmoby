using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.Locations.Queries.GetNeighborhoodById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetNeighborhoodsList
{
    public sealed class GetNeighborhoodsListQuery : IRequest<Result<PaginationResponse<NeighborhoodDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public Guid? CityId { get; set; }
        public string? Filter { get; set; }
    }

    public sealed class GetNeighborhoodsListUseCase : BaseHandler, IRequestHandler<GetNeighborhoodsListQuery, Result<PaginationResponse<NeighborhoodDTO>>>
    {
        private readonly INeighborhoodsRepository _repository;

        public GetNeighborhoodsListUseCase(INeighborhoodsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<NeighborhoodDTO>>> Handle(GetNeighborhoodsListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationResponse<Neighborhood> response = await _repository.GetPagedListAsync(query.Pagination,
                                                                                                 query.CityId,
                                                                                                 query.Filter);

                List<NeighborhoodDTO> items = response.Items.Select(neighborhood => new NeighborhoodDTO
                {
                    Id = neighborhood.Id,
                    Name = neighborhood.Name,
                    CityId = neighborhood.CityId
                }).ToList();

                return PaginationResponse<NeighborhoodDTO>.Create(items, response.TotalCount, query.Pagination);
            });
        }
    }
}
