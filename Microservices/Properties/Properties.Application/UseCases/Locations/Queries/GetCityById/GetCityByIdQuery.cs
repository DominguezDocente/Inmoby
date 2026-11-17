using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetCityById
{
    public sealed class GetCityByIdQuery : IRequest<Result<CityDTO>>
    {
        public Guid Id { get; set; }
    }

    public sealed class CityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid StateId { get; set; }
    }

    public sealed class GetCityByIdUseCase : BaseHandler, IRequestHandler<GetCityByIdQuery, Result<CityDTO>>
    {
        private readonly ICitiesRepository _repository;

        public GetCityByIdUseCase(ICitiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<CityDTO>> Handle(GetCityByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                City? city = await _repository.GetByIdAsync(query.Id);

                if (city is null)
                {
                    throw new NotFoundException($"No existe una ciudad con ID {query.Id}.");
                }

                return new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    StateId = city.StateId
                };
            });
        }
    }
}
