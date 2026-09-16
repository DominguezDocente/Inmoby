using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Configurations
{
    internal class PropertyImageConfig : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Url)
                   .HasMaxLength(1024)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(1024);

            builder.Property(p => p.IsPrimary)
                   .IsRequired();

            builder.Property(p => p.DisplayOrder)
                   .IsRequired();

            builder.HasOne(pi => pi.Property)
                  .WithMany(p => p.Images)
                  .HasForeignKey(p => p.PropertyId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
