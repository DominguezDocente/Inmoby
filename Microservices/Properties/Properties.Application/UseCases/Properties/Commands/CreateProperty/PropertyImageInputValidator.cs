using FluentValidation;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty
{
    public sealed class PropertyImageInputValidator : AbstractValidator<PropertyImageInput>
    {
        public PropertyImageInputValidator()
        {
            RuleFor(image => image.Url).NotEmpty().WithMessage("La URL de la imagen es requerida.")
                                       .MaximumLength(1024).WithMessage("La URL de la imagen no debe exceder los 1024 caracteres.");

            RuleFor(image => image.Description).MaximumLength(1024)
                                               .WithMessage("La descripcion de la imagen no debe exceder los 1024 caracteres.");

            RuleFor(image => image.DisplayOrder).GreaterThanOrEqualTo(0)
                                                .WithMessage("El orden de la imagen no puede ser negativo.");
        }
    }
}
