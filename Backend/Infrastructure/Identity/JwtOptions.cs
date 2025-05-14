namespace Backend.Infrastructure.Identity;
public class JwtOptions
{
    public const string JwtOptionsKey = "JWT";

    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
}
