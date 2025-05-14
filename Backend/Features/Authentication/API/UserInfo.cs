using Backend.Api;
using Backend.Core;
using Backend.Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Features.Authentication;


public partial class AuthController
{
    /// <summary>
    /// Returns the current user information if authenticated
    /// </summary>
    [HttpGet(EndpointRoutes.Auth.Session)]
    [Authorize(Policy = ApplicationPolicy.BasicPolicy)]

    [ProducesResponseType(typeof(ApiResponse<UserInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        // Check if the user is authenticated
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Not authenticated",
                Detail = "You must be logged in to access this resource"
            };
            return Unauthorized(problem);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User identity is authenticated but NameIdentifier claim is missing");
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Invalid authentication",
                Detail = "User identifier not found in authentication data"
            };
            return Unauthorized(problem);
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} from identity claims not found in database", userId);
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "User not found",
                Detail = "The authenticated user was not found in the system"
            };
            return Unauthorized(problem);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var response = new UserInfoResponse(user.Id.ToString(), user.Email!, roles);

        return Ok(ApiResponse<UserInfoResponse>.Success(response));
    }

}
public record UserInfoResponse(string UserId, string Email, IList<string> Roles);

