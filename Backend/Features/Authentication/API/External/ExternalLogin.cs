//AuthController.ExternalLogin.cs
using Backend.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication;

public partial class AuthController
{

    [HttpGet(EndpointRoutes.Auth.LoginGoogle)]
    [AllowAnonymous]
    public IActionResult ExternalLogin([FromServices] LinkGenerator linkGenerator, [FromQuery] string? returnUrl = null)
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google",
            linkGenerator.GetPathByName(HttpContext, "GoogleLoginCallback")
            + $"?returnUrl={returnUrl}");

        return Challenge(properties, ["Google"]);
    }
}
