using Booking.Api.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Booking.Api.Data.Entities
{
    public class Booking : IId
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public Guid UserId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; } = 30;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        [MaxLength(500)]
        public string? CancelReason { get; set; }
    }
}
