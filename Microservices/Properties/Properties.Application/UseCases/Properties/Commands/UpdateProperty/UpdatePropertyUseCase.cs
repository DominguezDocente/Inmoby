using Properties.Application.Contracts.Persistence;
using Properties.Application.Contracts.Repositories;
using Properties.Application.Exceptions;
using Properties.Application.UseCases.Common;
using Properties.Application.Utilities.Mediator;
using Properties.Application.Utilities.Results;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using Properties.Domain.Entities.Properties.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyUseCase : BaseHandler, IRequestHandler<UpdatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyUseCase(IPropertiesRepository propertiesRepository, IUnitOfWork unitOfWork)
        {
            _propertiesRepository = propertiesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdatePropertyCommand command)
        {
            return await ExecuteAsync(async () =>
            { 
                Property? property = await _propertiesRepository.GetByIdWithDetailsAsync(command.Id);

                if (property is null)
                {
                    throw new NotFoundException($"No existe propiedad con ID {command.Id}.");
                }

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
                             
                property.UpdateTitle(command.Title);
                property.UpdateDescription(command.Description);
                property.UpdateAddress(address);
                property.UpdatePrice(price);
                property.UpdateDetails(details);
                property.UpdatePrice(price);
                property.UpdatePropertyType(command.PropertyTypeId);
                property.UpdateNeighborhood(command.NeighborhoodId);
                property.UpdateAmenities(command.Amenities.Select(Amenity.FromCode));

                if (command.IsAvailable)
                {
                    property.MarkAsAvailable();
                }
                else
                {
                    property.MarkAsUnavailable();
                }

                await _propertiesRepository.UpdateAsync(property);
                await _unitOfWork.CommitAsync();


                return property.Id;
            });
        }
    }
}
