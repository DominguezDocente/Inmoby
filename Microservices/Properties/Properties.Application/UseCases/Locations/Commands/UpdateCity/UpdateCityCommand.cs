using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.UpdateCity
{
    public sealed class UpdateCityCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid StateId { get; set; }
    }

    public sealed class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("El ID de la ciudad es requerido.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre de la ciudad es requerido.")
                .MinimumLength(3).WithMessage("El nombre de la ciudad debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre de la ciudad no debe exceder los 64 caracteres.");

            RuleFor(command => command.StateId)
                .NotEmpty().WithMessage("El estado es requerido.");
        }
    }

    public sealed class UpdateCityUseCase : BaseHandler, IRequestHandler<UpdateCityCommand, Result<Guid>>
    {
        private readonly ICitiesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCityUseCase(ICitiesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateCityCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                City? city = await _repository.GetByIdAsync(command.Id);

                if (city is null)
                {
                    throw new NotFoundException($"No existe una ciudad con ID {command.Id}.");
                }

                city.UpdateName(command.Name);
                city.UpdateStateId(command.StateId);

                await _repository.UpdateAsync(city);
                await _unitOfWork.CommitAsync();

                return city.Id;
            });
        }
    }
}
