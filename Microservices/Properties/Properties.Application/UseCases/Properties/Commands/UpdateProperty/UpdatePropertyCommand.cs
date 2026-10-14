using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.UpdateProperty;

public sealed class UpdatePropertyCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }
    public string CurrencyCode { get; set; } = "COP";
    public Guid PropertyTypeId { get; set; }
    public Guid NeighborhoodId { get; set; }
    public PropertyAddressInput Address { get; set; } = new();
    public int Bedrooms { get; set; }
    public int ParkingSpaces { get; set; }
    public int Bathrooms { get; set; }
    public int Stratum { get; set; }
    public double Area { get; set; }
    public PropertiCondition Condition { get; set; }
    public bool IsAvailable { get; set; }
    public List<string> Amenities { get; set; } = [];
}
