using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertiesList
{
    internal static class MapperExtensions
    {
        public static PropertyListItemDTO ToListItemDTO(this Property property)
        {
            return new PropertyListItemDTO
            {
                Id = property.Id,
                Title = property.Title,
                Stratum = property.Details.Stratum,
                Area = property.Details.Area,
                BathRooms = property.Details.BathRooms,
                Bedrooms = property.Details.Bedrooms,   
                CityId = property.Address.CityId,
                CreatedAt = property.CreatedAt,
                ParkingSpaces = property.Details.ParkingSpaces,
                IsAvailable = property.IsAvailable,
                PriceAmount = property.Price.Amount,
                PriceCurrencyCode = property.Price.Type.Code,
                PropertyTypeId = property.PropertyTypeId,
                PropertyTypeName = property.PropertyType.Name,
                // TODO: Add CityName property to PropertyListItemDTO and map it here
                // CityName = property.Address.City.Name
            };
        }
    }
}
