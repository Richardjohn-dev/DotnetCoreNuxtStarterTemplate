// AuthController.Login.cs
using Backend.Api;
using Backend.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication;

public partial class AuthController
{

    // Login handler
    [HttpPost(EndpointRoutes.Auth.Login)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {

        var loginResult = await _userService.TryLogin(request);
        if (loginResult.IsSuccess == false)
            return ProblemResult(loginResult);

        AddAsHttpOnlyCookies(loginResult.Value.Tokens);
        return Ok(ApiResponse<UserInfoResponse>.Success(loginResult.Value.UserInfo));
    }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}


public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}



