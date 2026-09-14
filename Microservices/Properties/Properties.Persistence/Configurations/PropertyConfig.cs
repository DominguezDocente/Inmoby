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
        }
    }
}
