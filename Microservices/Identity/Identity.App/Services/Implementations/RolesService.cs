using Identity.App.DTOs.Responses;
using Identity.App.Models;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Identity.App.Services.Implementations;

public sealed class RolesService(RoleManager<AppRole> roleManager) : IRolesService
{
    public IReadOnlyCollection<RoleResponseDto> GetAllAsync()
    {
        IReadOnlyCollection<RoleResponseDto> roles = roleManager.Roles.OrderBy(role => role.Name)
                                                                      .Select(MapRole)
                                                                      .ToList();

        return roles;
    }

    private static RoleResponseDto MapRole(AppRole role) => new()
    {
        Id = role.Id,
        Name = role.Name ?? string.Empty
    };
}
