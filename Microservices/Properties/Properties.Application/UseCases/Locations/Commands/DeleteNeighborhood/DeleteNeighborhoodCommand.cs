using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.DeleteNeighborhood
{
    public sealed class DeleteNeighborhoodCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public sealed class DeleteNeighborhoodUseCase : BaseHandler, IRequestHandler<DeleteNeighborhoodCommand, Result<Guid>>
    {
        private readonly INeighborhoodsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNeighborhoodUseCase(INeighborhoodsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteNeighborhoodCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Neighborhood? neighborhood = await _repository.GetByIdAsync(command.Id);

                if (neighborhood is null)
                {
                    throw new NotFoundException($"No existe un barrio con ID {command.Id}.");
                }

                await _repository.DeleteAsync(command.Id);
                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
