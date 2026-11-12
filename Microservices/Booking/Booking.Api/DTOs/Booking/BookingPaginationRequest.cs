using Booking.Api.Core.Pagination;
using Booking.Api.Data.Entities;

namespace Booking.Api.DTOs.Booking
{
    public class BookingPaginationRequest : PaginationRequest
    {
        public BookingStatus? Status { get; set; }
        public Guid? PropertyId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
