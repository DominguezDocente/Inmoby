using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence.Configurations
{
    internal class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.HasOne(s => s.Country)
                  .WithMany(c => c.States)
                  .HasForeignKey(s => s.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
