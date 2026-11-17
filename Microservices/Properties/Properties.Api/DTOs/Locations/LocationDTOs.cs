using Properties.Api.DTOs.Properties;
using System.ComponentModel.DataAnnotations;

namespace Properties.Api.DTOs.Locations
{
    public class LocationNameDTO
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 64 caracteres.")]
        public string Name { get; set; } = string.Empty;
    }

    public sealed class CreateCountryDTO : LocationNameDTO
    {
    }

    public sealed class UpdateCountryDTO : LocationNameDTO
    {
    }

    public sealed class CreateStateDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "El país es requerido.")]
        public Guid CountryId { get; set; }
    }

    public sealed class UpdateStateDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "El país es requerido.")]
        public Guid CountryId { get; set; }
    }

    public sealed class CreateCityDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "El estado es requerido.")]
        public Guid StateId { get; set; }
    }

    public sealed class UpdateCityDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "El estado es requerido.")]
        public Guid StateId { get; set; }
    }

    public sealed class CreateNeighborhoodDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "La ciudad es requerida.")]
        public Guid CityId { get; set; }
    }

    public sealed class UpdateNeighborhoodDTO : LocationNameDTO
    {
        [NotEmptyGuid(ErrorMessage = "La ciudad es requerida.")]
        public Guid CityId { get; set; }
    }
}
