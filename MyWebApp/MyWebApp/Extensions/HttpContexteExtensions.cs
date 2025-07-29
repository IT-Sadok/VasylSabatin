using System.Security.Claims;

namespace MyWebApp.Extensions;

public static class HttpContexteExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("User ID is invalid or missing");

        return userId;
    }
}