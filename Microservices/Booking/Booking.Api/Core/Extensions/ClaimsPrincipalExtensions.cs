using System.Security.Claims;

namespace Booking.Api.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            string? value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? user.FindFirst("sub")?.Value;

            return Guid.TryParse(value, out Guid userId) ? userId : null;
        }
    }
}
