using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Properties.Api.Reponses;
using Properties.Application.Utilities.Results;

namespace Properties.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblemFromModelState<T>();
            }

            if (!result.IsSuccess)
            {
                return ToErrorActionResult<T>(result.Errors, result.Message);

            }

            return StatusCode(successStatusCode, ApiResponse<T>.Ok(result.Value!, result.Message));
        }

        private IActionResult ValidationProblemFromModelState<T>()
        {
            IReadOnlyList<ApiError> apiErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                 .Select(e => new ApiError("ValidationError", e.ErrorMessage, "validation"))
                                                                 .ToList();

            return BadRequest(ApiResponse<T>.Fail(apiErrors, "Debe ajustar los errores de validación"));
        }

        private IActionResult ToErrorActionResult<T>(IReadOnlyCollection<Error> errors, string? message)
        {
            ErrorType dominantType = errors.FirstOrDefault()?.Type ?? ErrorType.Unexpected;
            ApiResponse<T> body = ApiResponse<T>.Fail(ApiErrorMapper.FromMany(errors), message);

            return dominantType switch
            {
                ErrorType.Validation => StatusCode(StatusCodes.Status400BadRequest, body),
                ErrorType.Business => StatusCode(StatusCodes.Status400BadRequest, body),
                ErrorType.NotFound => StatusCode(StatusCodes.Status404NotFound, body),
                _ => StatusCode(StatusCodes.Status500InternalServerError, body)
            };
        }
    }
}
