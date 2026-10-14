using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.UseCases.Properties.Commands.CreateProperty;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.UpdateProperty;

public sealed class UpdatePropertyUseCase : BaseHandler, IRequestHandler<UpdatePropertyCommand, Result<Guid>>
{
    private readonly IPropertiesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePropertyUseCase(IPropertiesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task<Result<Guid>> Handle(UpdatePropertyCommand command) => ExecuteAsync(async () =>
    {
        var property = await _repository.GetByIdWithDetailsAsync(command.Id);
        if (property is null)
        {
            throw new NotFoundException($"No existe una propiedad con el identificador {command.Id}.");
        }

        PropertyAddressInput input = command.Address;
        Address address = new(
            input.MainRoadType,
            input.MainRoadNumber,
            input.MainRoadLetter,
            input.MainRoadSuffix,
            input.CrossRoadNumber,
            input.CrossRoadLetter,
            input.CrossRoadSuffix,
            input.Plate,
            input.Indications,
            input.PostalCode,
            input.Latitude,
            input.Longitude);

        Currency price = Currency.Create(command.PriceAmount, CurrencyType.FromCode(command.CurrencyCode));
        PropertyDetails details = new(
            command.Bedrooms,
            command.ParkingSpaces,
            command.Bathrooms,
            command.Stratum,
            command.Area,
            command.Condition);

        property.UpdateTitle(command.Title);
        property.UpdateDescription(command.Description);
        property.UpdatePrice(price);
        property.UpdatePropertyType(command.PropertyTypeId);
        property.UpdateNeighborhood(command.NeighborhoodId);
        property.UpdateAddress(address);
        property.UpdateDetails(details);
        property.UpdateAmenities(command.Amenities.Select(Amenity.FromCode));

        if (command.IsAvailable)
        {
            property.MarkAsAvailable();
        }
        else
        {
            property.MarkAsUnavailable();
        }

        await _unitOfWork.CommitAsync();
        return property.Id;
    });
}
