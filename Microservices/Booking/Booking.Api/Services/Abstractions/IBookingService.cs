using Booking.Api.Core;
using Booking.Api.Core.Pagination;
using Booking.Api.DTOs.Booking;

namespace Booking.Api.Services.Abstractions
{
    public interface IBookingService
    {
        Task<Result<BookingDTO>> CreateAsync(Guid userId, CreateBookingDTO dto);
        Task<Result<BookingDTO>> GetOneAsync(Guid id, Guid userId, bool isAdmin);
        Task<Result<PaginationResponse<BookingDTO>>> GetPaginationAsync(PaginationRequest request, Guid userId, bool isAdmin);
        Task<Result<BookingDTO>> UpdateAsync(Guid id, Guid userId, UpdateBookingDTO dto, bool isAdmin);
        Task<Result<BookingDTO>> CancelAsync(Guid id, Guid userId, CancelBookingDTO dto, bool isAdmin);
        Task<Result<BookingDTO>> ConfirmAsync(Guid id, Guid userId, bool isAdmin);
        Task<Result<BookingDTO>> CompleteAsync(Guid id, Guid userId, bool isAdmin);
    }
}
