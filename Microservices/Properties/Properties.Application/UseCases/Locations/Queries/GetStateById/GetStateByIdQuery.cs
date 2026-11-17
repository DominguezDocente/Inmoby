using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Queries.GetStateById
{
    public sealed class GetStateByIdQuery : IRequest<Result<StateDTO>>
    {
        public Guid Id { get; set; }
    }

    public sealed class StateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
    }

    public sealed class GetStateByIdUseCase : BaseHandler, IRequestHandler<GetStateByIdQuery, Result<StateDTO>>
    {
        private readonly IStatesRepository _repository;

        public GetStateByIdUseCase(IStatesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<StateDTO>> Handle(GetStateByIdQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                State? state = await _repository.GetByIdAsync(query.Id);

                if (state is null)
                {
                    throw new NotFoundException($"No existe un estado con ID {query.Id}.");
                }

                return new StateDTO
                {
                    Id = state.Id,
                    Name = state.Name,
                    CountryId = state.CountryId
                };
            });
        }
    }
}
