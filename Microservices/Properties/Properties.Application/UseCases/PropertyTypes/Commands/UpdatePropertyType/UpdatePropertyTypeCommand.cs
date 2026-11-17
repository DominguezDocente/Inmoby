using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.UseCases.PropertyTypes.Commands.UpdatePropertyType
{
    public sealed class UpdatePropertyTypeCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public sealed class UpdatePropertyTypeCommandValidator : AbstractValidator<UpdatePropertyTypeCommand>
    {
        public UpdatePropertyTypeCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("El ID del tipo de inmueble es requerido.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del tipo de inmueble es requerido.")
                .MinimumLength(3).WithMessage("El nombre del tipo de inmueble debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del tipo de inmueble no debe exceder los 64 caracteres.");

            RuleFor(command => command.Description)
                .MaximumLength(1024).WithMessage("La descripción no debe exceder los 1024 caracteres.");
        }
    }

    public sealed class UpdatePropertyTypeUseCase : BaseHandler, IRequestHandler<UpdatePropertyTypeCommand, Result<Guid>>
    {
        private readonly IPropertyTypesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyTypeUseCase(IPropertyTypesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdatePropertyTypeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                PropertyType? propertyType = await _repository.GetByIdAsync(command.Id);

                if (propertyType is null)
                {
                    throw new NotFoundException($"No existe un tipo de inmueble con ID {command.Id}.");
                }

                propertyType.Update(command.Name, command.Description);

                await _repository.UpdateAsync(propertyType);
                await _unitOfWork.CommitAsync();

                return propertyType.Id;
            });
        }
    }
}
