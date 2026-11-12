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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>().HasIndex(b => new { b.PropertyId, b.ScheduledAt});
            modelBuilder.Entity<BookingEntity>().HasIndex(b => new { b.UserId, b.ScheduledAt});

            base.OnModelCreating(modelBuilder);
        }
    }
}
