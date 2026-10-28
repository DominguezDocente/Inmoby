using Identity.App.DTOs.Requests;
using Identity.App.DTOs.Responses;
using Identity.App.Responses;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.App.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Register(RegisterUserDto request)
    {
        ServiceResult<UserResponseDto> result = await authService.RegisterAsync(request);
        if (result.IsSuccess)
            return Created("/api/auth/me", ApiResponse<UserResponseDto>.Ok(result.Data!, result.Message));

        return ToFailureResult<UserResponseDto>(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto request)
    {
        ServiceResult<AuthResponseDto> result = await authService.LoginAsync(request);
        if (result.IsSuccess)
            return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!, result.Message));

        return ToFailureResult<AuthResponseDto>(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Me()
    {
        ServiceResult<UserResponseDto> result = await authService.GetCurrentUserAsync(User);
        if (result.IsSuccess)
            return Ok(ApiResponse<UserResponseDto>.Ok(result.Data!, result.Message));

        return ToFailureResult<UserResponseDto>(result);
    }

    private ActionResult<ApiResponse<T>> ToFailureResult<T>(ServiceResult<T> result)
    {
        ApiError[] errors = result.Errors
            .Select(error => new ApiError(error.Code, error.Description, error.Type))
            .ToArray();
        ApiResponse<T> response = ApiResponse<T>.Fail(errors, result.Message);

        return result.Status switch
        {
            ServiceResultStatus.BadRequest => BadRequest(response),
            ServiceResultStatus.Conflict => Conflict(response),
            ServiceResultStatus.Unauthorized => Unauthorized(response),
            ServiceResultStatus.NotFound => NotFound(response),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response)
        };
    }
}
