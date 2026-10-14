using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertyById
{
    internal static class MapperExtensions
    {
        public static PropertyDetailsDTO ToDetailsDTO(this Property property)
        {
            return new PropertyDetailsDTO
            {
                Id = property.Id,
                Title = property.Title,
                Stratum = property.Details.Stratum,
                Area = property.Details.Area,
                Bathrooms = property.Details.BathRooms,
                Bedrooms = property.Details.Bedrooms,
                CreatedAt = property.CreatedAt,
                ParkingSpaces = property.Details.ParkingSpaces,
                PriceAmount = property.Price.Amount,
                CurrencyCode = property.Price.Type.Code,
                PropertyTypeId = property.PropertyTypeId,
                PropertyTypeName = property.PropertyType.Name,
                NeighborhoodId = property.NeighborhoodId,
                NeighborhoodName = property.Neighborhood!.Name,
                CityName = property.Neighborhood!.City.Name,
                CityId = property.Neighborhood.CityId,

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
                    Longitude = property.Address.Longitude,
                },

                Amenities = property.Amenities.Select(a => a.Name).ToList(),

                Images = property.Images.Select(i => new PropertyImageDTO
                {
                    Id = i.Id,
                    Url = i.Url,
                    Description = i.Description,
                    IsPrimary = i.IsPrimary,
                    DisplayOrder = i.DisplayOrder
                }).ToList(),
                
                Condition = property.Details.PropertiCondition,
                Description = property.Description,
                OwnerId = property.OwnerId
            };
        }
    }
}
