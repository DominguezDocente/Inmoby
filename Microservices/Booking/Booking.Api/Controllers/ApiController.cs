using Booking.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Response<T> response, int successStatusCode = StatusCodes.Status200OK)
        {
            if (response.IsSuccess)
            {
                return StatusCode(successStatusCode, response);
            }

            return StatusCode(response.StatusCode, response);
        }

        protected IActionResult ToActionResult(Response response, int successStatusCode = StatusCodes.Status200OK)
        {
            if (response.IsSuccess)
            {
                return StatusCode(successStatusCode, response);
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
