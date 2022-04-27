using System.Security.Claims;
namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var res = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if (res == null) return null;
        return int.Parse(res??"-1");
    }
}