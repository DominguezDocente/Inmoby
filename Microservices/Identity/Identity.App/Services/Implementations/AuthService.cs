using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.App.DTOs.Requests;
using Identity.App.DTOs.Responses;
using Identity.App.Models;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Identity.App.Services.Implementations;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IConfiguration configuration) : IAuthService
{
    public async Task<ServiceResult<UserResponseDto>> RegisterAsync(RegisterUserDto request)
    {
        string email = request.Email.Trim();
        AppUser user = new()
        {
            UserName = email,
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Document = request.Document.Trim().ToUpperInvariant()
        };

        IdentityResult createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            ServiceResultStatus status = createResult.Errors.Any(error => error.Code.Contains("Duplicate", StringComparison.OrdinalIgnoreCase))
                ? ServiceResultStatus.Conflict
                : ServiceResultStatus.BadRequest;
            return ServiceResult<UserResponseDto>.Failure(status, "The account could not be created.", MapErrors(createResult.Errors));
        }

        IdentityResult roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return ServiceResult<UserResponseDto>.Failure(
                ServiceResultStatus.Error,
                "The account could not be assigned its default role.",
                MapErrors(roleResult.Errors));
        }

        return ServiceResult<UserResponseDto>.Success(MapUser(user), "Account created successfully.");
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto request)
    {
        AppUser? user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null || !(await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true)).Succeeded)
        {
            ServiceError error = new("InvalidCredentials", "Invalid email or password.", "authentication");
            return ServiceResult<AuthResponseDto>.Failure(
                ServiceResultStatus.Unauthorized,
                "Invalid email or password.",
                [error]);
        }

        IList<string> roles = await userManager.GetRolesAsync(user);
        IConfigurationSection jwt = configuration.GetSection("Jwt");
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(jwt.GetValue<int>("ExpirationMinutes", 60));
        SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwt["SigningKey"]!));
        SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("document", user.Document),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email!)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        JwtSecurityToken token = new(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);
        AuthResponseDto response = new()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            TokenType = "Bearer",
            ExpiresAtUtc = expiresAt
        };

        return ServiceResult<AuthResponseDto>.Success(response, "Login successful.");
    }

    public async Task<ServiceResult<UserResponseDto>> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        AppUser? user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            ServiceError error = new("UserNotFound", "The authenticated user was not found.", "notfound");
            return ServiceResult<UserResponseDto>.Failure(
                ServiceResultStatus.NotFound,
                "The authenticated user was not found.",
                [error]);
        }

        return ServiceResult<UserResponseDto>.Success(MapUser(user, await userManager.GetRolesAsync(user)));
    }

    private static UserResponseDto MapUser(AppUser user, IEnumerable<string>? roles = null) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Document = user.Document,
        Email = user.Email!,
        Roles = roles ?? []
    };

    private static IReadOnlyCollection<ServiceError> MapErrors(IEnumerable<IdentityError> errors) =>
        errors.Select(error => new ServiceError(error.Code, error.Description, "identity")).ToArray();
}
