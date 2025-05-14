using Backend.Infrastructure.Persistence.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
           .Property(u => u.FullName).HasMaxLength(256);



        // Configure RefreshToken entity
        builder.Entity<RefreshToken>(entity =>
        {
            // Index on the token value for faster lookups
            entity.HasIndex(rt => rt.Token).IsUnique();

            entity.Property(rt => rt.Token).IsRequired().HasMaxLength(256);
            entity.Property(rt => rt.JwtId).IsRequired().HasMaxLength(256);
        });
    }
}

// create a migration
// dotnet ef migrations add "Intial IdentityDb" -o "Infrastructure/Persistence/Migrations"




// for migrations
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}