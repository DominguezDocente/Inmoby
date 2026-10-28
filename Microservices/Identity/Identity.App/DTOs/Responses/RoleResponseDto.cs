namespace Identity.App.DTOs.Responses;

public sealed class RoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
