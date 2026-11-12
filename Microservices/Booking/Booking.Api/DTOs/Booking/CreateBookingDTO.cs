using Booking.Api.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class CreateBookingDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime ScheduledAt { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(15, 120, ErrorMessage = "La duración debe estar entre {1} y {0} minutos")]
        public int DurationInMinutes { get; set; } = 30;

        [MaxLength(512, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Notes { get; set; }
    }
}
