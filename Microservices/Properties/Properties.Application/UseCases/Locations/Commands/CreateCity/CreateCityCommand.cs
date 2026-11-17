using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.CreateCity
{
    public sealed class CreateCityCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public Guid StateId { get; set; }
    }

    public sealed class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre de la ciudad es requerido.")
                .MinimumLength(3).WithMessage("El nombre de la ciudad debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre de la ciudad no debe exceder los 64 caracteres.");

            RuleFor(command => command.StateId)
                .NotEmpty().WithMessage("El estado es requerido.");
        }
    }

    public sealed class CreateCityUseCase : BaseHandler, IRequestHandler<CreateCityCommand, Result<Guid>>
    {
        private readonly ICitiesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCityUseCase(ICitiesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCityCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                City city = new(command.Name, command.StateId);

                await _repository.CreateAsync(city);
                await _unitOfWork.CommitAsync();

                return city.Id;
            });
        }
    }
}
