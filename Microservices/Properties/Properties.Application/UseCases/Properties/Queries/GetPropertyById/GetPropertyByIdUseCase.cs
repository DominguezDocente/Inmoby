using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById;

public sealed class GetPropertyByIdUseCase : BaseHandler, IRequestHandler<GetPropertyByIdQuery, Result<PropertyDetailsDTO>>
{
    private readonly IPropertiesRepository _repository;

    public GetPropertyByIdUseCase(IPropertiesRepository repository)
    {
        _repository = repository;
    }

    public Task<Result<PropertyDetailsDTO>> Handle(GetPropertyByIdQuery query) => ExecuteAsync(async () =>
    {
        var property = await _repository.GetByIdWithDetailsAsync(query.Id);
        if (property is null)
        {
            throw new NotFoundException($"No existe una propiedad con el identificador {query.Id}.");
        }

        return new PropertyDetailsDTO
        {
            Id = property.Id,
            OwnerId = property.OwnerId,
            Title = property.Title,
            Description = property.Description,
            PriceAmount = property.Price.Amount,
            CurrencyCode = property.Price.Type.Code,
            PropertyTypeId = property.PropertyTypeId,
            PropertyTypeName = property.PropertyType.Name,
            NeighborhoodId = property.NeighborhoodId,
            NeighborhoodName = property.Neighborhood.Name,
            CityId = property.Neighborhood.CityId,
            CityName = property.Neighborhood.City.Name,
            Address = new PropertyAddressDTO
            {
                MainRoadType = property.Address.MainRoadType,
                MainRoadNumber = property.Address.MainRoadNumber,
                MainRoadLetter = property.Address.MainRoadLetter,
                MainRoadSuffix = property.Address.MainRoadSuffix,
                CrossRoadNumber = property.Address.CrossRoadNumber,
                CrossRoadLetter = property.Address.CrossRoadLetter,
                CrossRoadSuffix = property.Address.CrossRoadSuffix,
                Plate = property.Address.Plate,
                Indications = property.Address.Indications,
                PostalCode = property.Address.PostalCode,
                Latitude = property.Address.Latitude,
                Longitude = property.Address.Longitude
            },
            Bedrooms = property.Details.Bedrooms,
            ParkingSpaces = property.Details.ParkingSpaces,
            Bathrooms = property.Details.BathRooms,
            Stratum = property.Details.Stratum,
            Area = property.Details.Area,
            Condition = property.Details.PropertiCondition,
            IsAvailable = property.IsAvailable,
            CreatedAt = property.CreatedAt,
            Amenities = property.Amenities.Select(amenity => amenity.Code).ToArray(),
            Images = property.Images.Select(image => new PropertyImageDTO
            {
                Id = image.Id,
                Url = image.Url,
                Description = image.Description,
                IsPrimary = image.IsPrimary,
                DisplayOrder = image.DisplayOrder
            }).ToArray()
        };
    });
}
