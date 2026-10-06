using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using Properties.Domain.Entities.Properties.ValueObjects;

namespace Properties.Application.UseCases.Properties.Commands.CreateProperty
{
    public sealed class CreatePropertyUseCase :
        BaseHandler,
        IRequestHandler<CreatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyUseCase(IPropertiesRepository propertiesRepository, IUnitOfWork unitOfWork)
        {
            _propertiesRepository = propertiesRepository;
            _unitOfWork = unitOfWork;
        }

        // TODO: Add PropertyImages
        public async Task<Result<Guid>> Handle(CreatePropertyCommand command)
        {
            return await ExecuteAsync(async () =>
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

                await _propertiesRepository.CreateAsync(property);
                await _unitOfWork.CommitAsync();

                return property.Id;
            });
        }
    }
}
