using System.ComponentModel.DataAnnotations;

namespace Booking.Api.DTOs.Booking
{
    public class CancelBookingDTO
    {
        [MaxLength(512, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Reason { get; set; }
    }
}
