using Identity.App.DTOs.User;
using Identity.App.Responses;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Identity.App.Services.Abstractions
{
    public interface IUsersService
    {
        Task<ApiResponse<List<UserDTO>>> GetAllAsync();
    }
}
