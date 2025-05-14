// Infrastructure/Common/CookieDefaults.cs
namespace Backend.Infrastructure.Common;

public static class CookieHelper
{

    public const string RefreshTokenName = "refresh_token";
    public const string AccessTokenName = "access_token";

    public static CookieOptions GetHttpOnlyCookie(DateTimeOffset expiration) => new()
    {
        HttpOnly = true,
        Secure = true,
        IsEssential = true,
        SameSite = SameSiteMode.Strict,
        Expires = expiration
    };

}