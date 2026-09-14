using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.UseCases.Properties.Queries.GetPropertiesList
{
    public class PropertyListItemDTO
    {
        public Guid Id { get;init; }
        public string Title { get;init; } = null!;
        public Guid CityId { get; set; }
        public string CityName { get; set; } = null!;
        public decimal PriceAmount { get;init; }
        public string PriceCurrencyCode { get;init; } = null!;
        public Guid PropertyTypeId { get;init; }
        public string PropertyTypeName { get;init; } = null!;
        public int Bedrooms { get;init; }
        public int ParkingSpaces { get;init; }
        public int BathRooms { get;init; }
        public int Stratum { get;init; }
        public double Area { get;init; }
        public bool IsAvailable { get;init; }
        public DateTime CreatedAt { get;init; }
    }
}
