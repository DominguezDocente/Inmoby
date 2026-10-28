namespace Identity.App.DTOs.Responses;

public sealed class UserResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = [];
}
