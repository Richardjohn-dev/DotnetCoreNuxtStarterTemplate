// AuthController.GoogleCallback.cs
using Backend.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Backend.Features.Authentication;

public partial class AuthController
{

    /// <summary>
    /// Handles the callback from external login providers
    /// </summary>
    [HttpGet(EndpointRoutes.Auth.LoginGoogleCallback, Name = "GoogleLoginCallback")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternaleCallback(string? returnUrl = null)
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Google authentication failed",
                    Detail = result.Failure?.Message,

                };
                return BadRequest(problem);
            }

            var tryGoogleLoginResult = await _userService.LoginWithGoogleAsync(result.Principal);
            if (tryGoogleLoginResult.IsSuccess == false)
            {
                return ProblemResult(tryGoogleLoginResult);

            }

            AddAsHttpOnlyCookies(tryGoogleLoginResult.Value.Tokens);
            return Redirect(GetFrontendRedirect(returnUrl, tryGoogleLoginResult.IsSuccess, tryGoogleLoginResult.Errors.FirstOrDefault()));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed google authentication: {ex.Message}");
            return Redirect(GetFrontendRedirect(returnUrl, false, "Authentication error occurred."));
        }

    }

    string GetFrontendRedirect(string? returnUrl, bool success, string? error)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "success", success.ToString().ToLowerInvariant() }
            };

        if (string.IsNullOrEmpty(error) == false)
            queryParams["error"] = error;


        return QueryHelpers.AddQueryString(GetFrontEndUrl(returnUrl), queryParams!);
    }
    private string GetFrontEndUrl(string? returnUrl)
    {
        var first = returnUrl is null ? "https://localhost:3000" : returnUrl;
        return $"{first}/auth/external-callback";
    }

}


