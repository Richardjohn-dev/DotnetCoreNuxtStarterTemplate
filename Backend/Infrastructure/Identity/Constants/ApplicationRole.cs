namespace Backend.Infrastructure.Identity.Constants;


public static class ApplicationRole
{
    public const string Admin = "Admin";
    public const string BasicUser = "BasicUser";
    public const string PremiumUser = "PremiumUser";

    public static string[] BasicAccessRoles = [Admin, PremiumUser, BasicUser];
    public static string[] PremiumAccessRoles = [Admin, PremiumUser];

    public static List<RoleDto> GetPublicRoles =>
     [
        new RoleDto
        {
            Role = Role.BasicUser,
            DisplayName = "Basic User",
            Description = "Standard user with basic permissions"
        },
        new RoleDto
        {
            Role = Role.PremiumUser,
            DisplayName = "Premium User",
            Description = "Premium user with additional features"
        }
    ];
}

public enum Role
{
    BasicUser = 0,
    PremiumUser = 1,
}


public class RoleDto
{
    public Role Role { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
}