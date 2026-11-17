using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.UpdateState
{
    public sealed class UpdateStateCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
    }

    public sealed class UpdateStateCommandValidator : AbstractValidator<UpdateStateCommand>
    {
        public UpdateStateCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("El ID del estado es requerido.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del estado es requerido.")
                .MinimumLength(3).WithMessage("El nombre del estado debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del estado no debe exceder los 64 caracteres.");

            RuleFor(command => command.CountryId)
                .NotEmpty().WithMessage("El país es requerido.");
        }
    }

    public sealed class UpdateStateUseCase : BaseHandler, IRequestHandler<UpdateStateCommand, Result<Guid>>
    {
        private readonly IStatesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStateUseCase(IStatesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateStateCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                State? state = await _repository.GetByIdAsync(command.Id);

                if (state is null)
                {
                    throw new NotFoundException($"No existe un estado con ID {command.Id}.");
                }

                state.UpdateName(command.Name);
                state.UpdateCountryId(command.CountryId);

                await _repository.UpdateAsync(state);
                await _unitOfWork.CommitAsync();

                return state.Id;
            });
        }
    }
}
