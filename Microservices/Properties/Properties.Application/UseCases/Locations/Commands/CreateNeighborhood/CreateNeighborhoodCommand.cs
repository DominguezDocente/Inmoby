using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.CreateNeighborhood
{
    public sealed class CreateNeighborhoodCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public Guid CityId { get; set; }
    }

    public sealed class CreateNeighborhoodCommandValidator : AbstractValidator<CreateNeighborhoodCommand>
    {
        public CreateNeighborhoodCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del barrio es requerido.")
                .MinimumLength(3).WithMessage("El nombre del barrio debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del barrio no debe exceder los 64 caracteres.");

            RuleFor(command => command.CityId)
                .NotEmpty().WithMessage("La ciudad es requerida.");
        }
    }

    public sealed class CreateNeighborhoodUseCase : BaseHandler, IRequestHandler<CreateNeighborhoodCommand, Result<Guid>>
    {
        private readonly INeighborhoodsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateNeighborhoodUseCase(INeighborhoodsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateNeighborhoodCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Neighborhood neighborhood = new(command.Name, command.CityId);

                await _repository.CreateAsync(neighborhood);
                await _unitOfWork.CommitAsync();

                return neighborhood.Id;
            });
        }
    }
}
