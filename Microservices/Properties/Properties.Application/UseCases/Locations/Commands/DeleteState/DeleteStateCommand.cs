using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.DeleteState
{
    public sealed class DeleteStateCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public sealed class DeleteStateUseCase : BaseHandler, IRequestHandler<DeleteStateCommand, Result<Guid>>
    {
        private readonly IStatesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStateUseCase(IStatesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteStateCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                State? state = await _repository.GetByIdAsync(command.Id);

                if (state is null)
                {
                    throw new NotFoundException($"No existe un estado con ID {command.Id}.");
                }

                await _repository.DeleteAsync(command.Id);
                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
