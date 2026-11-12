using Microsoft.EntityFrameworkCore;
using BookingEntity = Booking.Api.Data.Entities.Booking;

namespace Booking.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<BookingEntity> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookingEntity>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(b => b.Notes)
                      .HasMaxLength(500);

                entity.Property(b => b.CancelReason)
                      .HasMaxLength(500);

                entity.Property(b => b.ScheduledAt)
                      .IsRequired();

                entity.Property(b => b.CreatedAt)
                      .IsRequired();

                entity.HasIndex(b => new { b.PropertyId, b.ScheduledAt });
                entity.HasIndex(b => new { b.UserId, b.ScheduledAt });
                entity.HasIndex(b => b.Status);
            });

            base.OnModelCreating(builder);
        }
    }
}
