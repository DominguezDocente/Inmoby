using Azure;
using Booking.Api.Core;
using Booking.Api.Core.Extensions;
using Booking.Api.Core.Pagination;
using Booking.Api.DTOs.Booking;
using Booking.Api.Services.Abstractions;
using Booking.Api.Services.Implemetations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ApiController
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
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

            Result<PaginationResponse<BookingDTO>> response = await _bookingService.GetPaginationAsync(request,
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

            Result<BookingDTO> response = await _bookingService.GetOneAsync(id,
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

            Result<BookingDTO> response = await _bookingService.CreateAsync(userId.Value, dto);

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

            Result<BookingDTO> response = await _bookingService.UpdateAsync(id,
                                                                            userId.Value,
                                                                            dto,
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

            Result<BookingDTO> response = await _bookingService.ConfirmAsync(id,
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

            Result<BookingDTO> response = await _bookingService.CancelAsync(id,
                                                                              userId.Value,
                                                                              dto,
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

            Result<BookingDTO> response = await _bookingService.CompleteAsync(id,
                                                                                userId.Value,
                                                                                User.IsInRole("Admin"));

            return ToActionResult(response);
        }
    }
}
