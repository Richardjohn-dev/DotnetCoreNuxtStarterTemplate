using Backend.Core;
using Backend.Features.Authentication.Infrastructure;
using Backend.Infrastructure.Identity.Constants;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Persistence.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Features.Authentication.Application;


public record UserInfoDto(UserInfoResponse UserInfo, AccessTokensDto? Tokens);

public class AccountService
{
    private readonly AuthTokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountService> _logger;
    public AccountService(AuthTokenService tokenService, UserManager<User> userManager, ApplicationDbContext context, SignInManager<User> signInManager, ILogger<AccountService> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _context = context;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<Result<UserInfoDto>> TryRegister(RegisterRequest request)
    {
        try
        {

            var validator = new RegisterValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Result.ValidationFailure(validationResult.Errors);

            if (await EmailExists(request))
                return Result.Conflict("User Exists");


            var user = User.Create(request.Email, request.Fullname);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);

            var registerUserResult = await _userManager.CreateAsync(user);

            if (registerUserResult.Succeeded == false)
                return Result.InternalServerError(registerUserResult.Errors.Select(x => x.Description).ToArray());


            await _userManager.AddToRoleAsync(user, GetRoleName(request.Role));

            // Sign in the user immediately after registration
            await _signInManager.SignInAsync(user, isPersistent: true);

            return await CreateNewRefreshToken(user);

        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error registering: {ex}");
            return Result.InternalServerError($"Failed to login {ex.Message}");
        }
    }

    private async Task<bool> EmailExists(RegisterRequest request)
        => await _userManager.FindByEmailAsync(request.Email) != null;
    private string GetRoleName(Role role)
        => role switch
        {
            Role.BasicUser => ApplicationRole.BasicUser,
            Role.PremiumUser => ApplicationRole.PremiumUser,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Provided role is not supported.")
        };
    public async Task<Result<UserInfoDto>> LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal)
    {
        if (claimsPrincipal == null)
            return Result.Unauthorized("Google Login Failed: ClaimsPrincipal is missing");

        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        if (email == null)
            return Result.Unauthorized("Google Login Failed: Email is missing");

        var info = new UserLoginInfo("Google",
         claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
         "Google");

        var user = await _context.Users.Include(x => x.RefreshToken).SingleOrDefaultAsync(x => x.Email == email);


        if (user == null)
        {
            var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            var newUser = User.Create(email, name);
            newUser.EmailConfirmed = true;

            var registerUserResult = await _userManager.CreateAsync(newUser);

            if (registerUserResult.Succeeded == false)
                return Result.InternalServerError(registerUserResult.Errors.Select(x => x.Description).ToArray());

            await _userManager.AddToRoleAsync(newUser, ApplicationRole.BasicUser);

            var addLoginResult = await _userManager.AddLoginAsync(newUser, info);
            if (!addLoginResult.Succeeded)
                return Result.Unauthorized($"Google Login Failed: Email is missing {addLoginResult.Errors.Select(e => e.Description)}");


            user = newUser;
        }

        // confirm if we need to be using signinManager.
        //await _signInManager.SignInAsync(user, isPersistent: true);

        return string.IsNullOrEmpty(user.RefreshToken.Token) ? await CreateNewRefreshToken(user)
            : await RefreshTokenFor(user);
    }
    public async Task<Result<UserInfoDto>> TryLogin(LoginRequest request)
    {
        try
        {
            var validator = new LoginValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result.ValidationFailure(validationResult.Errors);
            }

            var user = await _context.Users
                .Include(rt => rt.RefreshToken)
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Result.Unauthorized("Provided email or password incorrect");
            }

            var signinResult = await _signInManager.PasswordSignInAsync(
               user,
               request.Password,
               isPersistent: true, // Enable "remember me" functionality 
               lockoutOnFailure: true
           );

            if (!signinResult.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user {Email}", request.Email);
                return Result.Unauthorized(GetSigninError(signinResult));
            }

            return await RefreshTokenFor(user);
        }

        catch (Exception ex)
        {
            _logger.LogWarning($"Error logging in: {ex}");
            return Result.InternalServerError($"Failed to login {ex.Message}");
        }
    }


    public async Task<Result<UserInfoDto>> TryGetRefresh(string? refreshTokenValueFromCookie)
    {
        if (string.IsNullOrEmpty(refreshTokenValueFromCookie))
            Result.Unauthorized("Refresh token is missing.");
        try
        {
            var storedToken = await _context.RefreshTokens
                                   .Include(rt => rt.User)
                                   .SingleOrDefaultAsync(rt => rt.Token == refreshTokenValueFromCookie);

            if (storedToken == null)
            {
                _logger.LogWarning("Refresh token from cookie not found in DB.");
                return Result.Unauthorized("Refresh token is missing or invalid.");
            }

            if (IsInvalid(storedToken))
            {
                _logger.LogWarning("Invalid refresh token used for User {UserId}. Revoked={IsRevoked}, Used={IsUsed}, Expired={IsExpired}",
                        storedToken.UserId, storedToken.IsRevoked, storedToken.IsUsed, storedToken.ExpiryDate < DateTime.UtcNow
                    );
                return Result.Unauthorized("Refresh token is missing or invalid.");
            }

            storedToken.IsUsed = true;

            return await RefreshTokenFor(storedToken.User);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error refreshing: {ex}");
            return Result.InternalServerError($"Failed to refresh token {ex.Message}");
        }
    }

    private async Task<Result<UserInfoDto>> RefreshTokenFor(User user)
    {
        if (user.RefreshToken is null)
            return Result.Unauthorized("No refresh token to update for user.");

        var userRoles = await _userManager.GetRolesAsync(user);

        // should update token not create
        var newToken = _tokenService.GetNewTokenDetails(user, userRoles);

        user.RefreshToken.Token = newToken.RefreshToken.Value;
        user.RefreshToken.JwtId = newToken.AccessToken.Jti;
        user.RefreshToken.IsUsed = false;
        user.RefreshToken.IsRevoked = false;
        user.RefreshToken.CreationDate = DateTime.UtcNow;
        user.RefreshToken.ExpiryDate = newToken.AccessToken.Expiration;


        _context.RefreshTokens.Update(user.RefreshToken);
        var saved = await _context.SaveChangesAsync() > 0;

        if (!saved)
            return Result.InternalServerError("Failed to save refresh token");

        var userInfo = new UserInfoResponse(user.Id.ToString(), user.Email!, userRoles);
        return new UserInfoDto(userInfo, newToken);
    }


    private static bool IsInvalid(RefreshToken storedToken)
    {
        return storedToken.IsRevoked || storedToken.IsUsed || storedToken.ExpiryDate < DateTime.UtcNow;
    }
    private async Task<Result<UserInfoDto>> CreateNewRefreshToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var newToken = _tokenService.GetNewTokenDetails(user, roles);

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newToken.RefreshToken.Value,
            JwtId = newToken.AccessToken.Jti,
            IsUsed = false,
            IsRevoked = false,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = newToken.AccessToken.Expiration
        };

        await _context.RefreshTokens.AddAsync(newRefreshTokenEntity);
        var saved = await _context.SaveChangesAsync() > 0;

        if (!saved)
            return Result.InternalServerError("Failed to save refresh token");


        var response = new UserInfoResponse(user.Id.ToString(), user.Email!, roles);

        return new UserInfoDto(response, newToken);
    }

    private string GetSigninError(SignInResult result) =>
     result switch
     {
         { IsLockedOut: true } => "Account is locked due to multiple failed attempts. Please try again later.",
         { IsNotAllowed: true } => "Account is not allowed to sign in. Email confirmation may be required.",
         _ => "The provided email or password is incorrect"
     };


}
