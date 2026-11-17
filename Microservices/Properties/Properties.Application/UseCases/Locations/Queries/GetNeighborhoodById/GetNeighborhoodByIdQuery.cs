using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetNeighborhoodById
{
    public sealed class GetNeighborhoodByIdQuery : IRequest<Result<NeighborhoodDTO>>
    {
        public Guid Id { get; set; }
    }

    public sealed class NeighborhoodDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CityId { get; set; }
    }

    public sealed class GetNeighborhoodByIdUseCase : BaseHandler, IRequestHandler<GetNeighborhoodByIdQuery, Result<NeighborhoodDTO>>
    {
        private readonly INeighborhoodsRepository _repository;

        public GetNeighborhoodByIdUseCase(INeighborhoodsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<NeighborhoodDTO>> Handle(GetNeighborhoodByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                Neighborhood? neighborhood = await _repository.GetByIdAsync(query.Id);

                if (neighborhood is null)
                {
                    throw new NotFoundException($"No existe un barrio con ID {query.Id}.");
                }

                return new NeighborhoodDTO
                {
                    Id = neighborhood.Id,
                    Name = neighborhood.Name,
                    CityId = neighborhood.CityId
                };
            });
        }
    }
}
