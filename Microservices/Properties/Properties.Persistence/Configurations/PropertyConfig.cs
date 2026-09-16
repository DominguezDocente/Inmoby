using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Configurations
{
    internal class PropertyConfig : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .IsRequired();

            builder.Property(p => p.OwnerId)
                   .IsRequired();

            builder.Property(p => p.IsAvailable)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(c => c.Amount)
                     .HasPrecision(18, 2)
                     .IsRequired();

                price.Property(c => c.Type)
                     .HasConversion(type => type.Code, code => CurrencyType.FromCode(code))
                     .HasMaxLength(3)
                     .IsRequired();
            });

            builder.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.MainRoadType).IsRequired();
                address.Property(a => a.MainRoadNumber).IsRequired()
                                                       .HasMaxLength(16);
                address.Property(a => a.MainRoadLetter);
                address.Property(a => a.MainRoadSuffix);
                address.Property(a => a.CrossRoadNumber).IsRequired();
                address.Property(a => a.CrossRoadLetter);
                address.Property(a => a.CountryId).IsRequired();
                address.Property(a => a.StateId).IsRequired();
                address.Property(a => a.CityId).IsRequired();
                address.Property(a => a.NeighborhoodId).IsRequired();
                address.Property(a => a.Plate);
                address.Property(a => a.PostalCode);
                address.Property(a => a.Latitude);
                address.Property(a => a.Longitude);
            });

            builder.HasOne(p => p.PropertyType)
                   .WithMany(pt => pt.Properties)
                   .HasForeignKey(p => p.PropertyTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(p => p.Details, details =>
            {
                details.Property(d => d.Bedrooms).IsRequired();
                details.Property(d => d.ParkingSpaces).IsRequired();
                details.Property(d => d.BathRooms).IsRequired();
                details.Property(d => d.Stratum).IsRequired();
                details.Property(d => d.Area).IsRequired();
            });

            builder.OwnsMany(p => p.Amenities, amenity =>
            {
                amenity.ToTable("PropertyAmenities");

                amenity.WithOwner()
                       .HasForeignKey("PropertyId");

                amenity.Property<Guid>("PropertyId");

                amenity.HasKey("PropertyId", "Code");

                amenity.Property(a => a.Code).HasMaxLength(64)
                                             .IsRequired();

                amenity.Property(a => a.Name).HasMaxLength(128)
                                             .IsRequired();
            });
        }
    }
}
