using Booking.Api.Core;
using Booking.Api.Core.Pagination;
using Booking.Api.DTOs.Booking;
using Booking.Api.Services.Abstractions;

namespace Booking.Api.Services.Implemetations
{
    public class BookingService : IBookingService
    {
        public Task<Result<BookingDTO>> CancelAsync(Guid id, Guid userId, CancelBookingDTO dto, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BookingDTO>> CompleteAsync(Guid id, Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BookingDTO>> ConfirmAsync(Guid id, Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BookingDTO>> CreateAsync(Guid userId, CreateBookingDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BookingDTO>> GetOneAsync(Guid id, Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaginationResponse<BookingDTO>>> GetPaginationAsync(PaginationRequest request, Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BookingDTO>> UpdateAsync(Guid id, Guid userId, UpdateBookingDTO dto, bool isAdmin)
        {
            throw new NotImplementedException();
        }
    }
}
