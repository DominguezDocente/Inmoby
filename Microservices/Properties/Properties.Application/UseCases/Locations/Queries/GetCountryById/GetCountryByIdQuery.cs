using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetCountryById
{
    public sealed class GetCountryByIdQuery : IRequest<Result<CountryDTO>>
    {
        public Guid Id { get; set; }
    }

    public sealed class CountryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public sealed class GetCountryByIdUseCase : BaseHandler, IRequestHandler<GetCountryByIdQuery, Result<CountryDTO>>
    {
        private readonly ICountriesRepository _repository;

        public GetCountryByIdUseCase(ICountriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<CountryDTO>> Handle(GetCountryByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                Country? country = await _repository.GetByIdAsync(query.Id);

                if (country is null)
                {
                    throw new NotFoundException($"No existe un país con ID {query.Id}.");
                }

                return new CountryDTO
                {
                    Id = country.Id,
                    Name = country.Name
                };
            });
        }
    }
}
