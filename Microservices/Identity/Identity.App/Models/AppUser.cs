using Microsoft.AspNetCore.Identity;

namespace Identity.App.Models;

public sealed class AppUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Document { get; set; }
}
