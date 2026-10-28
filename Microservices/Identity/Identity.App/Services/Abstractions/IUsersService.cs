using Identity.App.DTOs.Responses;
using Identity.App.Responses;

namespace Identity.App.Services.Abstractions;

public interface IUsersService
{
    Task<ApiResponse<IReadOnlyCollection<UserResponseDto>>> GetAllAsync();
    Task<ApiResponse<object?>> AssignRoleAsync(Guid userId, string roleName);
    Task<ApiResponse<object?>> RemoveRoleAsync(Guid userId, string roleName);
}
