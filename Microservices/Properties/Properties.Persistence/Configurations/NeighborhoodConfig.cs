using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Configurations
{
    internal class NeighborhoodConfig : IEntityTypeConfiguration<Neighborhood>
    {
        public void Configure(EntityTypeBuilder<Neighborhood> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.HasOne(n => n.City)
                   .WithMany(c => c.Neighborhoods)
                   .HasForeignKey(n => n.CityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
