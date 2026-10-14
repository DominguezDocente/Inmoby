using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using Properties.Domain.Entities.Properties.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById
{
    public sealed class PropertyDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public string CurrencyCode { get; set; }
        public Guid PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }
        public Guid NeighborhoodId { get; set; }
        public string NeighborhoodName { get; set; }
        public Guid CityId { get; set; } 
        public string CityName { get; set; }
        public int Bedrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public int Stratum { get; set; }
        public int Bathrooms { get; set; }
        public double Area { get; set; }
        public PropertiCondition Condition { get; set; }
        public DateTime CreatedAt { get; set; }
        public IReadOnlyCollection<string> Amenities { get; set; } = [];
        public IReadOnlyCollection<PropertyImageDTO> Images { get; set; } = [];
        public PropertyAddressDTO Address { get; set; } = new();
    }

    public class PropertyAddressDTO
    {
        public RoadTypeEnum MainRoadType { get; set; }
        public string MainRoadNumber { get; set; }
        public string? MainRoadLetter { get; set; }
        public RoadSuffixEnum? MainRoadSuffix { get; set; }
        public string CrossRoadNumber { get; set; }
        public string? CrossRoadLetter { get; set; }
        public RoadSuffixEnum? CrossRoadSuffix { get; set; }
        public string? Plate { get; set; }
        public string? Indications { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class PropertyImageDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
