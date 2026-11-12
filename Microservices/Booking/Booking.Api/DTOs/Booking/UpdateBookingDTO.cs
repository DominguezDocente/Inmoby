using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class UpdateBookingDTO
    {
        [Required(ErrorMessage = "La fecha de la visita es requerida.")]
        public DateTime ScheduledAt { get; set; }

        [Range(15, 240, ErrorMessage = "La duración debe estar entre 15 y 240 minutos.")]
        public int DurationMinutes { get; set; } = 30;

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string? Notes { get; set; }
    }
}
