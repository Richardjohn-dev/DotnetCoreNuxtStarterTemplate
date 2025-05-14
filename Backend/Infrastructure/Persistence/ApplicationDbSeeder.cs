using Backend.Features.Authentication.Infrastructure;
using Backend.Infrastructure.Identity.Constants;
using Backend.Infrastructure.Persistence.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence;
public class ApplicationDbSeeder
{
    private readonly AuthTokenService _tokenService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(
        ApplicationDbContext context,
        UserManager<User> userManager,
        ILogger<ApplicationDbSeeder> logger,
        RoleManager<IdentityRole<Guid>> roleManager,
        AuthTokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task ManageDataAsync(IConfiguration configuration)
    {
        await _context.Database.MigrateAsync();
        await SeedData(configuration);
        await SeedBasicAndPremiumUsers();
    }

    public async Task SeedData(IConfiguration configuration)
    {
        try
        {
            await EnsureRolesExist(ApplicationRole.Admin, ApplicationRole.BasicUser, ApplicationRole.PremiumUser);
            await EnsureAdminSeeded(configuration);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed seeding: {ex.Message}");

        }

    }

    private async Task EnsureAdminSeeded(IConfiguration configuration)
    {

        // Seed Admin User (get details from config/secrets)
        var adminEmail = configuration["AdminUser:Email"];
        var adminPassword = configuration["AdminUser:Password"];

        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            _logger.LogWarning("Admin User email or password not configured. Skipping admin user seed.");
            return;
        }

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = User.Create(adminEmail, "Admin");
            adminUser.EmailConfirmed = true;
            adminUser.PasswordHash = _userManager.PasswordHasher.HashPassword(adminUser, adminPassword);


            _logger.LogInformation("Creating admin user {AdminEmail}", adminEmail);
            var createAdminResult = await _userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminResult.Succeeded)
            {
                _logger.LogInformation("Assigning Admin role to {AdminEmail}", adminEmail);
                await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin);

                var userRoles = await _userManager.GetRolesAsync(adminUser);


                var newToken = _tokenService.GetNewTokenDetails(adminUser, userRoles);

                var newRefreshTokenEntity = new RefreshToken
                {
                    UserId = adminUser.Id,
                    Token = newToken.RefreshToken.Value,
                    JwtId = newToken.AccessToken.Jti,
                    IsUsed = false,
                    IsRevoked = false,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = newToken.AccessToken.Expiration
                };

                await _context.RefreshTokens.AddAsync(newRefreshTokenEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("Error creating admin user: {Errors}", string.Join(", ", createAdminResult.Errors.Select(e => e.Description)));
            }

        }
        else
        {
            _logger.LogInformation("Admin user {AdminEmail} already exists.", adminEmail);
        }
    }

    private async Task EnsureRolesExist(params string[] roles)
    {
        foreach (var roleName in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation("Creating role {RoleName}", roleName);
                await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }

    private async Task SeedBasicAndPremiumUsers()
    {
        var usersToSeed = new[]{
        new
        {
            Email = "john.basic@example.com",
            FullName = "John Smith (Basic User)",
            Role = ApplicationRole.BasicUser
        },
        new
        {
            Email = "jane.premium@example.com",
            FullName = "Jane Doe (Premium User)",
            Role = ApplicationRole.PremiumUser
        }};

        foreach (var userData in usersToSeed)
        {
            var existingUser = await _userManager.FindByEmailAsync(userData.Email);
            if (existingUser != null)
            {
                _logger.LogInformation("User {Email} already exists. Skipping.", userData.Email);
                continue;
            }

            var user = User.Create(userData.Email, userData.FullName);
            user.EmailConfirmed = true;

            const string defaultPassword = "password"; // Can also be sourced from config if preferred
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, defaultPassword);

            var createResult = await _userManager.CreateAsync(user, defaultPassword);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userData.Role);
                _logger.LogInformation("Created user {Email} and assigned role {Role}", userData.Email, userData.Role);

                var userRoles = await _userManager.GetRolesAsync(user);


                var newToken = _tokenService.GetNewTokenDetails(user, userRoles);

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
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("Failed to create user {Email}: {Errors}", userData.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
        }
    }

}




