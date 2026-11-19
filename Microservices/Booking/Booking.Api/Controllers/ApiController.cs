
using Booking.Api.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Result<T> response, int successStatusCode = StatusCodes.Status200OK)
        {
            if (response.IsSuccess)
            {
                return StatusCode(successStatusCode, response);
            }

            return StatusCode(response.StatusCode, response);
        }

        protected IActionResult ToActionResult(Result response, int successStatusCode = StatusCodes.Status200OK)
        {
            if (response.IsSuccess)
            {
                return StatusCode(successStatusCode, response);
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
