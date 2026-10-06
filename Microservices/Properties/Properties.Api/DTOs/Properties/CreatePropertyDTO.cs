using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Properties.Api.DTOs.Properties
{
    public class CreatePropertyDTO
    {
        [NotEmptyGuid]
        public Guid OwnerId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 4)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(32)]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999999999999.99")]
        public decimal PriceAmount { get; set; }

        [Required]
        public string CurrencyCode { get; set; } = "COP";

        [NotEmptyGuid]
        public Guid PropertyTypeId { get; set; }

        [NotEmptyGuid]
        public Guid NeighborhoodId { get; set; }

        [Required]
        public PropertyAddressRequest Address { get; set; } = new();

        [Range(1, int.MaxValue)]
        public int Bedrooms { get; set; }

        [Range(0, int.MaxValue)]
        public int ParkingSpaces { get; set; }

        [Range(1, int.MaxValue)]
        public int Bathrooms { get; set; }

        [Range(1, 6)]
        public int Stratum { get; set; }

        [Range(typeof(double), "20.000000000000004", "1.7976931348623157E+308")]
        public double Area { get; set; }

        [EnumDataType(typeof(PropertiCondition))]
        public PropertiCondition Condition { get; set; }

        public List<string> Amenities { get; set; } = [];
    }

    public sealed class PropertyAddressRequest : IValidatableObject
    {
        [EnumDataType(typeof(RoadTypeEnum))]
        public RoadTypeEnum MainRoadType { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string MainRoadNumber { get; set; } = string.Empty;

        [StringLength(4)]
        public string? MainRoadLetter { get; set; }

        [EnumDataType(typeof(RoadSuffixEnum))]
        public RoadSuffixEnum? MainRoadSuffix { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string CrossRoadNumber { get; set; } = string.Empty;

        [StringLength(4)]
        public string? CrossRoadLetter { get; set; }

        [EnumDataType(typeof(RoadSuffixEnum))]
        public RoadSuffixEnum? CrossRoadSuffix { get; set; }

        [StringLength(16)]
        public string? Plate { get; set; }

        public string? Indications { get; set; }

        [StringLength(8)]
        public string? PostalCode { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double? Latitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double? Longitude { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Latitude.HasValue != Longitude.HasValue)
            {
                yield return new ValidationResult(
                    "La latitud y la longitud deben enviarse juntas.",
                    [nameof(Latitude), nameof(Longitude)]);
            }
        }
    }

    public sealed class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => value is Guid id && id != Guid.Empty;
    }
}
