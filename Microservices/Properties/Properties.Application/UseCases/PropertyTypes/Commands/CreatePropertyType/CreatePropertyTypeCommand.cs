using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Properties;

namespace Properties.Application.UseCases.PropertyTypes.Commands.CreatePropertyType
{
    public sealed class CreatePropertyTypeCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public sealed class CreatePropertyTypeCommandValidator : AbstractValidator<CreatePropertyTypeCommand>
    {
        public CreatePropertyTypeCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del tipo de inmueble es requerido.")
                .MinimumLength(3).WithMessage("El nombre del tipo de inmueble debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del tipo de inmueble no debe exceder los 64 caracteres.");

            RuleFor(command => command.Description)
                .MaximumLength(1024).WithMessage("La descripción no debe exceder los 1024 caracteres.");
        }
    }

    public sealed class CreatePropertyTypeUseCase : BaseHandler, IRequestHandler<CreatePropertyTypeCommand, Result<Guid>>
    {
        private readonly IPropertyTypesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyTypeUseCase(IPropertyTypesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreatePropertyTypeCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                PropertyType propertyType = new(command.Name, command.Description);

                await _repository.CreateAsync(propertyType);
                await _unitOfWork.CommitAsync();

                return propertyType.Id;
            });
        }
    }
}
