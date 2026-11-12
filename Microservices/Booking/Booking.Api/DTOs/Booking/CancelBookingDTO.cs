using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class CancelBookingDTO
    {
        [MaxLength(500, ErrorMessage = "El motivo no puede superar los 500 caracteres.")]
        public string? Reason { get; set; }
    }
}
