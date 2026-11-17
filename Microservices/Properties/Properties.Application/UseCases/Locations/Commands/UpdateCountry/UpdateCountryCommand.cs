using FluentValidation;
using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Entities.Locations;

namespace Properties.Application.UseCases.Locations.Commands.UpdateCountry
{
    public sealed class UpdateCountryCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public sealed class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("El ID del país es requerido.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("El nombre del país es requerido.")
                .MinimumLength(3).WithMessage("El nombre del país debe tener al menos 3 caracteres.")
                .MaximumLength(64).WithMessage("El nombre del país no debe exceder los 64 caracteres.");
        }
    }

    public sealed class UpdateCountryUseCase : BaseHandler, IRequestHandler<UpdateCountryCommand, Result<Guid>>
    {
        private readonly ICountriesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateCountryCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Country? country = await _repository.GetByIdAsync(command.Id);

                if (country is null)
                {
                    throw new NotFoundException($"No existe un país con ID {command.Id}.");
                }

                country.UpdateName(command.Name);

                await _repository.UpdateAsync(country);
                await _unitOfWork.CommitAsync();

                return country.Id;
            });
        }
    }
}
