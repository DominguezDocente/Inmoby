using Identity.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.App.Data;

public sealed class IdentitySeeder(
    IdentityDbContext db,
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager)
{
    private static readonly string[] RoleNames = ["User", "Admin", "Properties Manager"];

    private static readonly SeedUser[] Users =
    [
        new("admin@inmoby.local", "Admin123", "System", "Administrator", "SEED-ADMIN", "Admin"),
        new("properties@inmoby.local", "Properties123", "Properties", "Manager", "SEED-PROPERTIES-MANAGER", "Properties Manager")
    ];

    public async Task SeedAsync()
    {
        await db.Database.EnsureCreatedAsync();
        //await db.Database.MigrateAsync();

        foreach (string roleName in RoleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            IdentityResult roleResult = await roleManager.CreateAsync(new AppRole(roleName));
            EnsureSuccess(roleResult, $"Could not create role '{roleName}'");
        }

        foreach (SeedUser seedUser in Users)
            await SeedUserAsync(userManager, seedUser);
    }

    private static async Task SeedUserAsync(UserManager<AppUser> userManager, SeedUser seedUser)
    {
        AppUser? user = await userManager.FindByEmailAsync(seedUser.Email);
        if (user is null)
        {
            user = new AppUser
            {
                UserName = seedUser.Email,
                Email = seedUser.Email,
                EmailConfirmed = true,
                FirstName = seedUser.FirstName,
                LastName = seedUser.LastName,
                Document = seedUser.Document
            };

            IdentityResult createResult = await userManager.CreateAsync(user, seedUser.Password);
            EnsureSuccess(createResult, $"Could not create the base '{seedUser.RoleName}' user");
        }

        if (!await userManager.IsInRoleAsync(user, seedUser.RoleName))
        {
            IdentityResult roleResult = await userManager.AddToRoleAsync(user, seedUser.RoleName);
            EnsureSuccess(roleResult, $"Could not assign the '{seedUser.RoleName}' role to {seedUser.Email}");
        }
    }

    private static void EnsureSuccess(IdentityResult result, string message)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException($"{message}: {string.Join("; ", result.Errors.Select(error => error.Description))}");
    }

    private sealed record SeedUser(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Document,
        string RoleName);
}
