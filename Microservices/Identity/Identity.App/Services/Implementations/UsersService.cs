using Identity.App.Models;
using Identity.App.DTOs.Responses;
using Identity.App.Responses;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.App.Services.Implementations;

public sealed class UsersService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : IUsersService
{
    public async Task<ApiResponse<IReadOnlyCollection<UserResponseDto>>> GetAllAsync()
    {
        List<AppUser> users = await userManager.Users
            .OrderBy(user => user.Email)
            .ToListAsync();

        List<UserResponseDto> response = new();
        foreach (AppUser user in users)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            response.Add(new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Document = user.Document,
                Email = user.Email!,
                Roles = roles
            });
        }

        return ApiResponse<IReadOnlyCollection<UserResponseDto>>.Ok(response);
    }

    public async Task<ApiResponse<object?>> AssignRoleAsync(Guid userId, string roleName)
    {
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null || !await roleManager.RoleExistsAsync(roleName))
        {
            ApiError error = new("UserOrRoleNotFound", "The user or role was not found.", "notfound");
            return ApiResponse<object?>.Fail([error], "The requested user or role was not found.");
        }

        IdentityResult result = await userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            ApiError[] errors = result.Errors.Select(error => new ApiError(error.Code, error.Description, "identity")).ToArray();
            return ApiResponse<object?>.Fail(errors, "The role could not be assigned.");
        }

        return ApiResponse<object?>.Ok(null, "Role assigned successfully.");
    }

    public async Task<ApiResponse<object?>> RemoveRoleAsync(Guid userId, string roleName)
    {
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null || !await roleManager.RoleExistsAsync(roleName))
        {
            ApiError error = new("UserOrRoleNotFound", "The user or role was not found.", "notfound");
            return ApiResponse<object?>.Fail([error], "The requested user or role was not found.");
        }

        IdentityResult result = await userManager.RemoveFromRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            ApiError[] errors = result.Errors.Select(error => new ApiError(error.Code, error.Description, "identity")).ToArray();
            return ApiResponse<object?>.Fail(errors, "The role could not be removed.");
        }

        return ApiResponse<object?>.Ok(null, "Role removed successfully.");
    }
}
