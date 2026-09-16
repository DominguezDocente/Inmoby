using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Configurations
{
    internal class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.HasOne(c => c.State)
                   .WithMany(s => s.Cities)
                   .HasForeignKey(c => c.StateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
