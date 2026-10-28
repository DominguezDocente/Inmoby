using Identity.App.Models;
using Identity.App.DTOs.Responses;
using Identity.App.Responses;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.App.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UserResponseDto>>>> GetAll()
    {
        ApiResponse<IReadOnlyCollection<UserResponseDto>> response = await usersService.GetAllAsync();
        return Ok(response);
    }

    [HttpPut("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> AssignRole(Guid userId, string roleName)
    {
        ApiResponse<object?> response = await usersService.AssignRoleAsync(userId, roleName);
        return ToActionResult(response);
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        ApiResponse<object?> response = await usersService.RemoveRoleAsync(userId, roleName);
        return ToActionResult(response);
    }

    private IActionResult ToActionResult(ApiResponse<object?> response)
    {
        if (response.IsSuccess) return Ok(response);
        if (response.Errors?.Any(error => error.Type == "notfound") == true) return NotFound(response);
        return BadRequest(response);
    }
}

