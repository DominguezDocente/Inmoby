using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.UseCases.PropertyTypes.Commands.DeletePropertyType
{
    public sealed class DeletePropertyTypeCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public sealed class DeletePropertyTypeUseCase : BaseHandler, IRequestHandler<DeletePropertyTypeCommand, Result<Guid>>
    {
        private readonly IPropertyTypesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyTypeUseCase(IPropertyTypesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeletePropertyTypeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                PropertyType? propertyType = await _repository.GetByIdAsync(command.Id);

                if (propertyType is null)
                {
                    throw new NotFoundException($"No existe un tipo de inmueble con ID {command.Id}.");
                }

                await _repository.DeleteAsync(command.Id);
                await _unitOfWork.CommitAsync();

                return command.Id;
            });
        }
    }
}
