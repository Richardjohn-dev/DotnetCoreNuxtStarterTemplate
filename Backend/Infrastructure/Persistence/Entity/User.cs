using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Persistence.Entity;
public class User : IdentityUser<Guid>
{
    public required string FullName { get; set; }

    public static User Create(string email, string fullname)
    {
        return new User
        {
            Email = email,
            UserName = email,
            FullName = fullname,
        };
    }

    public override string ToString() => FullName;

    public RefreshToken RefreshToken { get; set; }
}