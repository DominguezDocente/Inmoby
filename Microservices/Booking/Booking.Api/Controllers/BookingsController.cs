using Booking.Api.Core;
using Booking.Api.Core.Extensions;
using Booking.Api.Core.Pagination;
using Booking.Api.DTOs.Booking;
using Booking.Api.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ApiController
    {
        private readonly IBookingsService _bookingsService;

        public BookingsController(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] BookingPaginationRequest request)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            bool isAdmin = User.IsInRole("Admin");

            Response<PaginationResponse<BookingDTO>> response = await _bookingsService.GetPaginationAsync(request,
                                                                                                         userId.Value,
                                                                                                         isAdmin);

            return ToActionResult(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.GetOneAsync(id,
                                                                              userId.Value,
                                                                              User.IsInRole("Admin"));

            return ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDTO dto)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.CreateAsync(userId.Value, dto);

            return ToActionResult(response, StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookingDTO dto)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.UpdateAsync(id,
                                                                              dto,
                                                                              userId.Value,
                                                                              User.IsInRole("Admin"));

            return ToActionResult(response);
        }

        [HttpPost("{id:guid}/confirm")]
        public async Task<IActionResult> Confirm([FromRoute] Guid id)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.ConfirmAsync(id,
                                                                               userId.Value,
                                                                               User.IsInRole("Admin"));

            return ToActionResult(response);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelBookingDTO dto)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.CancelAsync(id,
                                                                              dto,
                                                                              userId.Value,
                                                                              User.IsInRole("Admin"));

            return ToActionResult(response);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete([FromRoute] Guid id)
        {
            Guid? userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            Response<BookingDTO> response = await _bookingsService.CompleteAsync(id,
                                                                                userId.Value,
                                                                                User.IsInRole("Admin"));

            return ToActionResult(response);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response response = await _bookingsService.DeleteAsync(id);

            return ToActionResult(response);
        }
    }
}
