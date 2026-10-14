using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Properties.Api.DTOs.Properties
{
    public class UpdatePropertyDTO
    {
        [NotEmptyGuid]
        [Required]
        public Guid Id { get; set; }

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

        [Required]
        public bool IsAvailable { get; set; }
    }
}

