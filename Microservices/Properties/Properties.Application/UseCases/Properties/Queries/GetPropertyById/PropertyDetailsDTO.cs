using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById;

public sealed class PropertyDetailsDTO
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal PriceAmount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public Guid PropertyTypeId { get; init; }
    public string PropertyTypeName { get; init; } = string.Empty;
    public Guid NeighborhoodId { get; init; }
    public string NeighborhoodName { get; init; } = string.Empty;
    public Guid CityId { get; init; }
    public string CityName { get; init; } = string.Empty;
    public PropertyAddressDTO Address { get; init; } = new();
    public int Bedrooms { get; init; }
    public int ParkingSpaces { get; init; }
    public int Bathrooms { get; init; }
    public int Stratum { get; init; }
    public double Area { get; init; }
    public PropertiCondition Condition { get; init; }
    public bool IsAvailable { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<string> Amenities { get; init; } = [];
    public IReadOnlyCollection<PropertyImageDTO> Images { get; init; } = [];
}

public sealed class PropertyAddressDTO
{
    public RoadTypeEnum MainRoadType { get; init; }
    public string MainRoadNumber { get; init; } = string.Empty;
    public string? MainRoadLetter { get; init; }
    public RoadSuffixEnum? MainRoadSuffix { get; init; }
    public string CrossRoadNumber { get; init; } = string.Empty;
    public string? CrossRoadLetter { get; init; }
    public RoadSuffixEnum? CrossRoadSuffix { get; init; }
    public string? Plate { get; init; }
    public string? Indications { get; init; }
    public string? PostalCode { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}

public sealed class PropertyImageDTO
{
    public Guid Id { get; init; }
    public string Url { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsPrimary { get; init; }
    public int DisplayOrder { get; init; }
}
