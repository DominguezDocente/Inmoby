using FluentValidation;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty
{
    public sealed class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(cmd => cmd.OwnerId).NotEmpty().WithMessage("El propietario es requerido.");

            RuleFor(cmd => cmd.Title).NotEmpty().WithMessage("El título es requerido.")
                                     .MinimumLength(4).WithMessage("El título debe tener al menos 4 caracteres.")
                                     .MaximumLength(256).WithMessage("El título no debe exceder los 256 caracteres.");

            RuleFor(cmd => cmd.Description).NotEmpty().WithMessage("La descripción es requerida.")
                                           .MinimumLength(32).WithMessage("La descripción debe tener al menos 32 caracteres.");

            RuleFor(cmd => cmd.PriceAmount).GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(cmd => cmd.CurrencyCode).Must(IsSupportedCurrencyType).WithMessage("El código de moneda no es válido.");

            RuleFor(cmd => cmd.PropertyTypeId).NotEmpty().WithMessage("El tipo de propiedad es requerido.");

            RuleFor(cmd => cmd.NeighborhoodId).NotEmpty().WithMessage("El barrio es requerido.");

            RuleFor(cmd => cmd.Bedrooms).GreaterThan(0).WithMessage("La propiedad debe tener al menos una habitación.");

            RuleFor(cmd => cmd.ParkingSpaces).GreaterThanOrEqualTo(0).WithMessage("Los espacios de parqueo no pueden ser negativos.");

            RuleFor(cmd => cmd.Stratum).InclusiveBetween(1, 6).WithMessage("El estrato debe estar entre 1 y 6.");

            RuleFor(cmd => cmd.Area).GreaterThan(20).WithMessage("El área debe tener al menos 20 metros cuadrados.");

            RuleFor(command => command.Condition).IsInEnum().WithMessage("La condición de la propiedad no es válida.");

            RuleFor(cmd => cmd.Amenities).NotNull().WithMessage("Las comodidades son requeridas.")
                                         .Must(HaveUniqueAmenities).WithMessage("No se puede repetir una comodidad.");

            RuleForEach(cmd => cmd.Amenities).NotEmpty().WithMessage("La comodidad es requerida")
                                             .Must(IsSupportedAmenity).WithMessage("Uno de los códigos de comodidad no es válido.");

            RuleFor(cmd => cmd.Address).NotNull().WithMessage("La dirección es requerida.")
                                       .SetValidator(new PropertyAddressInputValidator());
        }

        private static bool IsSupportedCurrencyType(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            return CurrencyType.All.Any(type => type.Code.Equals(code.Trim()));
        }

        private static bool IsSupportedAmenity(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            string normalizedCode = code.Replace(" ", "_")
                                        .ToUpper();

            return Amenity.All.Any(amenity => amenity.Code.Equals(normalizedCode));
        }

        private static bool HaveUniqueAmenities(List<string>? codes)
        {
            if (codes is null)
            {
                return false;
            }

            IEnumerable<string> normalizedCodes = codes.Where(code => !string.IsNullOrWhiteSpace(code))
                                                       .Select(code => code.Replace(" ", "_").ToUpper());

            return normalizedCodes.Distinct().Count() == normalizedCodes.Count();
        }
    }
}
