// AuthController.RefreshToken.cs
using Backend.Api;
using Backend.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication;

public partial class AuthController
{
    /// <summary>
    /// Refreshes the authentication tokens
    /// </summary>
    [HttpPost(EndpointRoutes.Auth.Refresh)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken()
    {

        var tryRefresh = await _userService.TryGetRefresh(Request.Cookies["refresh_token"]);

        if (tryRefresh.IsSuccess == false)
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return ProblemResult(tryRefresh);
        }

        AddAsHttpOnlyCookies(tryRefresh.Value.Tokens);
        return Ok(ApiResponse<bool>.Success("Token refreshed successfully."));

    }
}


