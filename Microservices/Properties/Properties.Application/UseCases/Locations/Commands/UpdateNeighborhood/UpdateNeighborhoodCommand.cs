using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.UpdateNeighborhood
{
    public sealed class UpdateNeighborhoodCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CityId { get; set; }
    }

    public sealed class UpdateNeighborhoodCommandValidator : AbstractValidator<UpdateNeighborhoodCommand>
    {
        public UpdateNeighborhoodCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("El ID del barrio es requerido.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del barrio es requerido.")
                .MinimumLength(3).WithMessage("El nombre del barrio debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del barrio no debe exceder los 64 caracteres.");

            RuleFor(command => command.CityId)
                .NotEmpty().WithMessage("La ciudad es requerida.");
        }
    }

    public sealed class UpdateNeighborhoodUseCase : BaseHandler, IRequestHandler<UpdateNeighborhoodCommand, Result<Guid>>
    {
        private readonly INeighborhoodsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNeighborhoodUseCase(INeighborhoodsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateNeighborhoodCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Neighborhood? neighborhood = await _repository.GetByIdAsync(command.Id);

                if (neighborhood is null)
                {
                    throw new NotFoundException($"No existe un barrio con ID {command.Id}.");
                }

                neighborhood.UpdateName(command.Name);
                neighborhood.UpdateCityId(command.CityId);

                await _repository.UpdateAsync(neighborhood);
                await _unitOfWork.CommitAsync();

                return neighborhood.Id;
            });
        }
    }
}
