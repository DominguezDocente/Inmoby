using Microsoft.AspNetCore.Identity;

namespace Identity.App.Models;

public sealed class AppRole : IdentityRole<Guid>
{
    public AppRole() { }
    public AppRole(string name) : base(name) { }
}
