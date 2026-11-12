using System.ComponentModel.DataAnnotations;

namespace Booking.Api.Data.Entities
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public Guid UserId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationInMinutes { get; set; } = 30;

        public BookingStatusEnum Status { get; set; }
        
        [MaxLength(512)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        [MaxLength(512)]
        public string? CancelReason { get; set; }
    }
}
