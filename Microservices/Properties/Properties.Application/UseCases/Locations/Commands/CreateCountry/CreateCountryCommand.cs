using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.CreateCountry
{
    public sealed class CreateCountryCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
    }

    public sealed class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del país es requerido.")
                .MinimumLength(3).WithMessage("El nombre del país debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del país no debe exceder los 64 caracteres.");
        }
    }

    public sealed class CreateCountryUseCase : BaseHandler, IRequestHandler<CreateCountryCommand, Result<Guid>>
    {
        private readonly ICountriesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCountryCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Country country = new(command.Name);

                await _repository.CreateAsync(country);
                await _unitOfWork.CommitAsync();

                return country.Id;
            });
        }
    }
}
