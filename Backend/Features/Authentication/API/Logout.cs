// AuthController.Logout.cs
using Backend.Api;
using Backend.Core;
using Backend.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication;

public partial class AuthController
{

    [HttpPost(EndpointRoutes.Auth.Logout)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        Response.HttpContext.Response.Cookies.Delete(CookieHelper.RefreshTokenName);
        Response.HttpContext.Response.Cookies.Delete(CookieHelper.AccessTokenName);
        return Ok(ApiResponse<bool>.Success("Logged out successfully."));
    }
}