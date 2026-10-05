using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Utilities.Mediator;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty;

public sealed class CreatePropertyUseCase : IRequestHandler<CreatePropertyCommand, Guid>
{
    private readonly IPropertiesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePropertyUseCase(IPropertiesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePropertyCommand command)
    {
        PropertyAddressInput addressInput = command.Address;
        Address address = new(
            addressInput.MainRoadType,
            addressInput.MainRoadNumber,
            addressInput.MainRoadLetter,
            addressInput.MainRoadSuffix,
            addressInput.CrossRoadNumber,
            addressInput.CrossRoadLetter,
            addressInput.CrossRoadSuffix,
            addressInput.Plate,
            addressInput.Indications,
            addressInput.PostalCode,
            addressInput.Latitude,
            addressInput.Longitude);

        Currency price = Currency.Create(command.PriceAmount, CurrencyType.FromCode(command.CurrencyCode));
        PropertyDetails details = new(
            command.Bedrooms,
            command.ParkingSpaces,
            command.Bathrooms,
            command.Stratum,
            command.Area,
            command.Condition);

        Property property = new(
            command.OwnerId,
            command.Title,
            command.Description,
            price,
            command.PropertyTypeId,
            address,
            isAvailable: true,
            details,
            command.NeighborhoodId);

        foreach (string amenityCode in command.Amenities)
        {
            property.AddAmenity(Amenity.FromCode(amenityCode));
        }

        await _repository.CreateAsync(property);
        await _unitOfWork.CommitAsync();

        return property.Id;
    }
}
