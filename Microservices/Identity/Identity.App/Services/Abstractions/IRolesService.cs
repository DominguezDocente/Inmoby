using Identity.App.DTOs.Responses;

namespace Identity.App.Services.Abstractions;

public interface IRolesService
{
    IReadOnlyCollection<RoleResponseDto> GetAllAsync();
}
