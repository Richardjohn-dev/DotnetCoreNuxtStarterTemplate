using Backend.Infrastructure.Persistence;
using Backend.Startup;


//var logger = new LoggerConfiguration()

//                .WriteTo.File("logs/errorlog-.txt",
//                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
//                .CreateLogger();
try
{
    // Log.Information("Starting web application"); // Serilog

    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDependencies();

    var app = builder.Build();
    await MigrateDatabase(app, builder.Configuration);

    // Configure middleware pipeline
    app.ConfigureMiddlewarePipeline();
    app.AddSampleRoleEndpoints();


    app.Run();

}
catch (Exception ex)
{
    var sdfs = "";
    //Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    //Log.Information("Closing service complete");
    //Log.CloseAndFlush();
}

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













//builder.Services.AddOpenApi();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//if (string.IsNullOrEmpty(connectionString))
//    throw new InvalidOperationException("Database connection string 'DefaultConnection' not configured.");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//      options.UseSqlServer(connectionString));

//builder.Services.AddScoped<ApplicationDbSeeder>();


//builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 4;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;

//    //options.Password.RequireDigit = true;
//    //options.Password.RequiredLength = 8;
//    //options.Password.RequireLowercase = true;
//    //options.Password.RequireUppercase = true;
//    //options.Password.RequireNonAlphanumeric = false;

//    //options.SignIn.RequireConfirmedAccount = true;

//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
//    options.Lockout.MaxFailedAccessAttempts = 5;

//    options.User.RequireUniqueEmail = true;

//}).AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

//builder.Services.AddScoped<AuthTokenService>();
//builder.Services.AddScoped<AccountService>();

//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddCookie().AddGoogle(options =>
//{
//    // add to secrets later.
//    var googleConfig = builder.Configuration.GetSection("Authentication:Google");
//    var clientId = googleConfig["ClientId"];
//    var clientSecret = googleConfig["ClientSecret"];

//    ArgumentNullException.ThrowIfNull(clientId, nameof(clientId));
//    ArgumentNullException.ThrowIfNull(clientSecret, nameof(clientSecret));


//    options.ClientId = clientId;
//    options.ClientSecret = clientSecret;
//    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

//}).AddJwtBearer(options =>
//{
//    var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
//        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtOptions.Issuer,
//        ValidAudience = jwtOptions.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
//    };

//    // attach as cookie
//    options.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            context.Token = context.Request.Cookies[CookieHelper.AccessTokenName];
//            return Task.CompletedTask;
//        }
//    };
//});


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(ApplicationPolicy.BasicPolicy, policy => policy.RequireRole(ApplicationRole.BasicAccessRoles));
//    options.AddPolicy(ApplicationPolicy.PremiumPolicy, policy => policy.RequireRole(ApplicationRole.PremiumAccessRoles));
//    options.AddPolicy(ApplicationPolicy.AdminPolicy, policy => policy.RequireRole(ApplicationRole.Admin));


//    //// You can add more complex policies as needed
//    //options.AddPolicy("RequireConfirmedAccount", policy =>
//    //    policy.RequireAssertion(context =>
//    //        context.User.HasClaim(c =>
//    //            c.Type == "EmailConfirmed" && c.Value == "true")));

//});

//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddControllers();

//builder.RegisterServices();

//// Exception handling
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.MapOpenApi();
//    app.MapScalarApiReference(opt =>
//    {
//        opt.WithTitle("JWT + Refresh Token Auth API");
//        opt.Theme = ScalarTheme.Saturn;
//        opt.Layout = ScalarLayout.Modern;
//        opt.DarkMode = false;
//        opt.HideClientButton = true;
//    });
//}
//else
//{
//    app.UseExceptionHandler();
//    app.UseHsts();
//}

//app.UseExceptionHandler(_ => { });
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();



//app.MapGet(pattern: "/api/public",
//   () => Results.Ok(new List<string> { "Everyone can see this." }));

//app.MapGet(pattern: "/api/basic",
//    () => Results.Ok(new List<string> { "Thanks for being a member." }))
//            .RequireAuthorization();

//app.MapGet(pattern: "/api/premium",
//    () => Results.Ok(new List<string> { "Thanks for being a premium member." }))
//            .RequireAuthorization(ApplicationPolicy.PremiumPolicy);

//app.MapGet(pattern: "/api/admin",
//   () => Results.Ok(new List<string> { "Super secret company data" }))
//           .RequireAuthorization(ApplicationPolicy.AdminPolicy);
