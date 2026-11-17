using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.CreateState
{
    public sealed class CreateStateCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
    }

    public sealed class CreateStateCommandValidator : AbstractValidator<CreateStateCommand>
    {
        public CreateStateCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del estado es requerido.")
                .MinimumLength(3).WithMessage("El nombre del estado debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del estado no debe exceder los 64 caracteres.");

            RuleFor(command => command.CountryId)
                .NotEmpty().WithMessage("El país es requerido.");
        }
    }

    public sealed class CreateStateUseCase : BaseHandler, IRequestHandler<CreateStateCommand, Result<Guid>>
    {
        private readonly IStatesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStateUseCase(IStatesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateStateCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                State state = new(command.Name, command.CountryId);

                await _repository.CreateAsync(state);
                await _unitOfWork.CommitAsync();

                return state.Id;
            });
        }
    }
}
