using Identity.App.DTOs.Responses;
using Identity.App.Responses;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.App.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class RolesController(IRolesService rolesService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<IReadOnlyCollection<RoleResponseDto>>> GetAll()
    {
        IReadOnlyCollection<RoleResponseDto> roles = rolesService.GetAllAsync();
        return Ok(ApiResponse<IReadOnlyCollection<RoleResponseDto>>.Ok(roles));
    }
}
