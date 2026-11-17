using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.DeleteCity
{
    public sealed class DeleteCityCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public sealed class DeleteCityUseCase : BaseHandler, IRequestHandler<DeleteCityCommand, Result<Guid>>
    {
        private readonly ICitiesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCityUseCase(ICitiesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteCityCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                City? city = await _repository.GetByIdAsync(command.Id);

                if (city is null)
                {
                    throw new NotFoundException($"No existe una ciudad con ID {command.Id}.");
                }

                await _repository.DeleteAsync(command.Id);
                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
