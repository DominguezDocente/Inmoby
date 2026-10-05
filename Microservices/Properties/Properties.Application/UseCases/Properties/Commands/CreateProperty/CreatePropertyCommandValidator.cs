using FluentValidation;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty;

public sealed class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(command => command.OwnerId)
            .NotEmpty().WithMessage("El propietario es obligatorio.");

        RuleFor(command => command.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MinimumLength(4).WithMessage("El título debe tener al menos 4 caracteres.")
            .MaximumLength(256).WithMessage("El título debe tener máximo 256 caracteres.");

        RuleFor(command => command.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MinimumLength(32).WithMessage("La descripción debe tener al menos 32 caracteres.");

        RuleFor(command => command.PriceAmount)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

        RuleFor(command => command.CurrencyCode)
            .Must(IsSupportedCurrency)
            .WithMessage("El código de moneda debe ser COP, USD o EUR.");

        RuleFor(command => command.PropertyTypeId)
            .NotEmpty().WithMessage("El tipo de propiedad es obligatorio.");

        RuleFor(command => command.NeighborhoodId)
            .NotEmpty().WithMessage("El barrio es obligatorio.");

        RuleFor(command => command.Address)
            .NotNull().WithMessage("La dirección es obligatoria.")
            .SetValidator(new PropertyAddressInputValidator());

        RuleFor(command => command.Bedrooms)
            .GreaterThan(0).WithMessage("La propiedad debe tener al menos una habitación.");

        RuleFor(command => command.ParkingSpaces)
            .GreaterThanOrEqualTo(0).WithMessage("Los espacios de parqueo no pueden ser negativos.");

        RuleFor(command => command.Bathrooms)
            .GreaterThan(0).WithMessage("La propiedad debe tener al menos un baño.");

        RuleFor(command => command.Stratum)
            .InclusiveBetween(1, 6).WithMessage("El estrato debe estar entre 1 y 6.");

        RuleFor(command => command.Area)
            .GreaterThan(20).WithMessage("El área debe ser mayor a 20 metros cuadrados.");

        RuleFor(command => command.Condition)
            .IsInEnum().WithMessage("La condición de la propiedad no es válida.");

        RuleFor(command => command.Amenities)
            .NotNull().WithMessage("La lista de comodidades no puede ser nula.");

        RuleForEach(command => command.Amenities)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El código de comodidad es obligatorio.")
            .Must(IsSupportedAmenity)
            .WithMessage("Uno de los códigos de comodidad no es válido.");

        RuleFor(command => command.Amenities)
            .Must(HaveUniqueAmenities)
            .WithMessage("No se puede repetir una comodidad.");
    }

    private static bool IsSupportedCurrency(string? code) =>
        !string.IsNullOrWhiteSpace(code) &&
        CurrencyType.All.Any(currency => currency.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));

    private static bool IsSupportedAmenity(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }

        string normalizedCode = code.Replace(' ', '_');
        return Amenity.All.Any(amenity => amenity.Code.Equals(normalizedCode, StringComparison.OrdinalIgnoreCase));
    }

    private static bool HaveUniqueAmenities(List<string>? codes)
    {
        if (codes is null)
        {
            return false;
        }

        IEnumerable<string> normalizedCodes = codes
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.Replace(' ', '_'));

        return normalizedCodes.Distinct(StringComparer.OrdinalIgnoreCase).Count() == normalizedCodes.Count();
    }
}
