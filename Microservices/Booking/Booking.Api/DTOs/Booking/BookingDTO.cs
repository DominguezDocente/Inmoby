using Booking.Api.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class BookingDTO
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public Guid UserId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationInMinutes { get; set; } = 30;

        public BookingStatusEnum Status { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        public string? CancelReason { get; set; }
    }
}
