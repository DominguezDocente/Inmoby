using System.ComponentModel.DataAnnotations;

namespace Properties.Api.DTOs.PropertyTypes
{
    public sealed class CreatePropertyTypeDTO
    {
        [Required(ErrorMessage = "El nombre del tipo de inmueble es requerido.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "El nombre del tipo de inmueble debe tener entre 3 y 64 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1024, ErrorMessage = "La descripción no debe exceder los 1024 caracteres.")]
        public string? Description { get; set; }
    }

    public sealed class UpdatePropertyTypeDTO
    {
        [Required(ErrorMessage = "El nombre del tipo de inmueble es requerido.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "El nombre del tipo de inmueble debe tener entre 3 y 64 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1024, ErrorMessage = "La descripción no debe exceder los 1024 caracteres.")]
        public string? Description { get; set; }
    }
}
