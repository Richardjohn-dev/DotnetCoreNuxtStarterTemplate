using Backend.Infrastructure.Identity;
using Backend.Infrastructure.Persistence.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Features.Authentication.Infrastructure;

public class AuthTokenService
{
    private readonly JwtOptions _jwtOptions;

    public AuthTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }


    public AccessTokensDto GetNewTokenDetails(User user, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jti = Guid.NewGuid().ToString(); // Unique JWT ID



        IEnumerable<Claim> claims = [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, jti),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (ClaimTypes.NameIdentifier, user.ToString()),
            ..roles.Select(role => new Claim(ClaimTypes.Role, role))
          ];


        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        var accessToken = new AccessTokenDto(jwtToken, jti, expires);
        var refreshToken = new RefreshTokenDto(GenerateRefreshTokenValue());
        return new AccessTokensDto(accessToken, refreshToken);
    }



    internal static string GenerateRefreshTokenValue(int length = 64)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

