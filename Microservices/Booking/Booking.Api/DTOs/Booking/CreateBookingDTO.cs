using Booking.Api.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class CreateBookingDTO
    {
        [NotEmptyGuid(ErrorMessage = "El inmueble es requerido.")]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "La fecha de la visita es requerida.")]
        public DateTime ScheduledAt { get; set; }

        [Range(15, 240, ErrorMessage = "La duración debe estar entre 15 y 240 minutos.")]
        public int DurationMinutes { get; set; } = 30;

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string? Notes { get; set; }
    }
}
