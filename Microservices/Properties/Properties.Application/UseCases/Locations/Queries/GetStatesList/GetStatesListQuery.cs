using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.Locations.Queries.GetStateById;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Pagination;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetStatesList
{
    public sealed class GetStatesListQuery : IRequest<Result<PaginationResponse<StateDTO>>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public Guid? CountryId { get; set; }
        public string? Filter { get; set; }
    }

    public sealed class GetStatesListUseCase : BaseHandler, IRequestHandler<GetStatesListQuery, Result<PaginationResponse<StateDTO>>>
    {
        private readonly IStatesRepository _repository;

        public GetStatesListUseCase(IStatesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<StateDTO>>> Handle(GetStatesListQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                PaginationResponse<State> response = await _repository.GetPagedListAsync(query.Pagination,
                                                                                         query.CountryId,
                                                                                         query.Filter);

                List<StateDTO> items = response.Items.Select(state => new StateDTO
                {
                    Id = state.Id,
                    Name = state.Name,
                    CountryId = state.CountryId
                }).ToList();

                return PaginationResponse<StateDTO>.Create(items, response.TotalCount, query.Pagination);
            });
        }
    }
}
