using Backend.Infrastructure.Persistence;
using Backend.Startup;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDependencies();

var app = builder.Build();
await MigrateDatabase(app, builder.Configuration);

// Configure middleware pipeline
app.ConfigureMiddlewarePipeline();
app.AddSampleRoleEndpoints();


app.Run();




static async Task MigrateDatabase(WebApplication app, IConfiguration config)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var seeder = services.GetRequiredService<ApplicationDbSeeder>();
        await seeder.ManageDataAsync(config);

        // Optional logging
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database");
        throw;
    }
}




