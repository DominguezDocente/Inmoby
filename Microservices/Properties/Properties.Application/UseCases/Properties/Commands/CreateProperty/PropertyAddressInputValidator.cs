using FluentValidation;
using Properties.Domain.Common.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty;

public sealed class PropertyAddressInputValidator : AbstractValidator<PropertyAddressInput>
{
    public PropertyAddressInputValidator()
    {
        RuleFor(address => address.MainRoadType)
            .IsInEnum().WithMessage("El tipo de vía principal no es válido.");

        RuleFor(address => address.MainRoadNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El número de la vía principal es obligatorio.")
            .MaximumLength(16).WithMessage("El número de la vía principal no puede exceder 16 caracteres.");

        RuleFor(address => address.MainRoadLetter)
            .MaximumLength(4).WithMessage("La letra de la vía principal no puede exceder 4 caracteres.");

        RuleFor(address => address.MainRoadSuffix)
            .Must(suffix => !suffix.HasValue || Enum.IsDefined(suffix.Value))
            .WithMessage("El sufijo de la vía principal no es válido.");

        RuleFor(address => address.CrossRoadNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El número de la vía que cruza es obligatorio.")
            .MaximumLength(16).WithMessage("El número de la vía que cruza no puede exceder 16 caracteres.");

        RuleFor(address => address.CrossRoadLetter)
            .MaximumLength(4).WithMessage("La letra de la vía que cruza no puede exceder 4 caracteres.");

        RuleFor(address => address.CrossRoadSuffix)
            .Must(suffix => !suffix.HasValue || Enum.IsDefined(suffix.Value))
            .WithMessage("El sufijo de la vía que cruza no es válido.");

        RuleFor(address => address.Plate)
            .MaximumLength(16).WithMessage("La placa no puede exceder 16 caracteres.");

        RuleFor(address => address.PostalCode)
            .MaximumLength(8).WithMessage("El código postal no puede exceder 8 caracteres.");

        RuleFor(address => address)
            .Must(HaveBothOrNoCoordinates)
            .WithMessage("La latitud y la longitud deben enviarse juntas.");

        RuleFor(address => address.Latitude)
            .InclusiveBetween(-90, 90)
            .When(address => address.Latitude.HasValue)
            .WithMessage("La latitud debe estar entre -90 y 90 grados.");

        RuleFor(address => address.Longitude)
            .InclusiveBetween(-180, 180)
            .When(address => address.Longitude.HasValue)
            .WithMessage("La longitud debe estar entre -180 y 180 grados.");
    }

    private static bool HaveBothOrNoCoordinates(PropertyAddressInput address) =>
        address.Latitude.HasValue == address.Longitude.HasValue;
}
