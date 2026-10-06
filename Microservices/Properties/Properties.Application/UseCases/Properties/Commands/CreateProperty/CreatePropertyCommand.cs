using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty
{
    public sealed class CreatePropertyCommand : IRequest<Result<Guid>>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public string CurrencyCode { get; set; }
        public Guid PropertyTypeId { get; set; }
        public Guid NeighborhoodId { get; set; }
        public int Bedrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public int Stratum { get; set; }
        public int Bathrooms { get; set; }
        public double Area { get; set; }
        public PropertiCondition Condition { get; set; }
        public List<string> Amenities { get; set; }
        public PropertyAddressInput Address { get; set; } = new();
    }

    public sealed class PropertyAddressInput
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
}
