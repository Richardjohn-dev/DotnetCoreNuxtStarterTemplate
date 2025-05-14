// AuthController.Register.cs
using Backend.Api;
using Backend.Core;
using Backend.Infrastructure.Identity.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication;

public partial class AuthController
{
    /// <summary>
    /// Handles user registration
    /// </summary>
    [HttpPost(EndpointRoutes.Auth.Register)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var registerResult = await _userService.TryRegister(request);

        if (registerResult.IsSuccess == false)
            return ProblemResult(registerResult);

        AddAsHttpOnlyCookies(registerResult.Value.Tokens);
        return Ok(ApiResponse<UserInfoResponse>.Success(registerResult.Value.UserInfo));
    }
}

public record RegisterRequest(string Email, string Fullname, string Password, string ConfirmPassword, Role Role);


public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8); // Match Identity settings
        RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}