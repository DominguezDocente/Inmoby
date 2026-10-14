using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;

namespace Properties.Application.UseCases.Properties.Commands.DeleteProperty;

public sealed class DeletePropertyUseCase : BaseHandler, IRequestHandler<DeletePropertyCommand, Result<Guid>>
{
    private readonly IPropertiesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePropertyUseCase(IPropertiesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task<Result<Guid>> Handle(DeletePropertyCommand command) => ExecuteAsync(async () =>
    {
        bool deleted = await _repository.DeleteWithImagesAsync(command.Id);
        if (!deleted)
        {
            throw new NotFoundException($"No existe una propiedad con el identificador {command.Id}.");
        }

        await _unitOfWork.CommitAsync();
        return command.Id;
    });
}
