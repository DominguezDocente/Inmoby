using Identity.App.DTOs.Auth;

namespace Identity.App.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    }
}
