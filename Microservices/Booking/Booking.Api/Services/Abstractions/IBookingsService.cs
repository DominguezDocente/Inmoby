using Booking.Api.Core;
using Booking.Api.Core.Pagination;
using Booking.Api.DTOs.Booking;

namespace Booking.Api.Services.Abstractions
{
    public interface IBookingsService
    {
        Task<Response<BookingDTO>> CreateAsync(Guid userId, CreateBookingDTO dto);
        Task<Response<BookingDTO>> GetOneAsync(Guid id, Guid userId, bool isAdmin);
        Task<Response<PaginationResponse<BookingDTO>>> GetPaginationAsync(BookingPaginationRequest request, Guid userId, bool isAdmin);
        Task<Response<BookingDTO>> UpdateAsync(Guid id, UpdateBookingDTO dto, Guid userId, bool isAdmin);
        Task<Response<BookingDTO>> ConfirmAsync(Guid id, Guid userId, bool isAdmin);
        Task<Response<BookingDTO>> CancelAsync(Guid id, CancelBookingDTO dto, Guid userId, bool isAdmin);
        Task<Response<BookingDTO>> CompleteAsync(Guid id, Guid userId, bool isAdmin);
        Task<Response> DeleteAsync(Guid id);
    }
}
