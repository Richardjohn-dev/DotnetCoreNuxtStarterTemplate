// AuthController.cs
using Backend.Features.Authentication.Application;
using Backend.Features.Shared;
using Backend.Infrastructure.Common;
using Backend.Infrastructure.Identity;
using Backend.Infrastructure.Persistence.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace Backend.Features.Authentication;

public partial class AuthController : BaseController
{
    private readonly AccountService _userService;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<User> _userManager;

    public AuthController(IOptions<JwtOptions> jwtOptions,
        SignInManager<User> signInManager,
        ILogger<AuthController> logger,
        AccountService userService,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userService = userService;
        _userManager = userManager;
    }

    private void AddAsHttpOnlyCookies(AccessTokensDto? tokens)
    {
        if (tokens is not null)
        {
            AddAccessHttpOnlyCookie(tokens.AccessToken);
            AddRefreshTokenHttpOnlyCookie(tokens.RefreshToken);
        }
    }

    private void AddRefreshTokenHttpOnlyCookie(RefreshTokenDto refreshToken)
        => AppendHttpOnlyCookie(refreshToken.CookieName, refreshToken.Value, refreshToken.Expiration);

    private void AddAccessHttpOnlyCookie(AccessTokenDto accessToken)
        => AppendHttpOnlyCookie(accessToken.CookieName, accessToken.Value, accessToken.Expiration);

    private void AppendHttpOnlyCookie(string cookieName, string token, DateTimeOffset expiration)
         => Response.Cookies.Append(cookieName, token, CookieHelper.GetHttpOnlyCookie(expiration));

}
public record AccessTokensDto(AccessTokenDto AccessToken, RefreshTokenDto RefreshToken);
public record AccessTokenDto(string Value, string Jti, DateTimeOffset Expiration)
{
    public string CookieName => CookieHelper.AccessTokenName;
};
public record RefreshTokenDto(string Value)
{
    public DateTimeOffset Expiration => DateTime.UtcNow.AddDays(7);
    public string CookieName => CookieHelper.RefreshTokenName;
};