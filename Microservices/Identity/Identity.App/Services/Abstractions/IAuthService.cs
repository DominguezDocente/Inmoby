using System.Security.Claims;
using Identity.App.DTOs.Requests;
using Identity.App.DTOs.Responses;

namespace Identity.App.Services.Abstractions;

public interface IAuthService
{
    Task<ServiceResult<UserResponseDto>> RegisterAsync(RegisterUserDto request);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto request);
    Task<ServiceResult<UserResponseDto>> GetCurrentUserAsync(ClaimsPrincipal principal);
}

public enum ServiceResultStatus
{
    Success,
    BadRequest,
    Conflict,
    Unauthorized,
    NotFound,
    Error
}

public sealed record ServiceError(string Code, string Description, string Type);

public sealed record ServiceResult<T>(
    ServiceResultStatus Status,
    T? Data,
    string? Message,
    IReadOnlyCollection<ServiceError> Errors)
{
    public bool IsSuccess => Status == ServiceResultStatus.Success;

    public static ServiceResult<T> Success(T data, string? message = null) =>
        new(ServiceResultStatus.Success, data, message, []);

    public static ServiceResult<T> Failure(
        ServiceResultStatus status,
        string message,
        IReadOnlyCollection<ServiceError>? errors = null) =>
        new(status, default, message, errors ?? []);
}
