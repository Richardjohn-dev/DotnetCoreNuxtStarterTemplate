using Backend.Features.Authentication.Application;
using Backend.Features.Authentication.Infrastructure;
using Backend.Infrastructure;
using Backend.Infrastructure.Common;
using Backend.Infrastructure.Identity;
using Backend.Infrastructure.Identity.Constants;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Persistence.Entity;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Startup;


public static class ServicesRegistrationExtensions
{
    public static WebApplicationBuilder AddServiceDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services.ConfigureErrorHandling();
        builder.Services.ConfigureDatabase(builder.Configuration);

        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureAuthentication(builder.Configuration);
        builder.Services.ConfigureAuthorizationPolicies();

        // CORS policies
        builder.Services.ConfigureSPACors(builder.Configuration);

        // Add controllers - this is the new line you need to add
        builder.Services.AddControllers();

        // FastEndpoints and Swagger
        builder.Services.ConfigureFastEndpoints();

        return builder;
    }

    public static IServiceCollection ConfigureErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options => options.CustomizeProblemDetails = problemContext =>
        {
            problemContext.ProblemDetails.Instance = $"{problemContext.HttpContext.Request.Method} {problemContext.HttpContext.Request.Path}";
            problemContext.ProblemDetails.Extensions.TryAdd("requestId", problemContext.HttpContext.TraceIdentifier);
            var activity = problemContext.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            problemContext.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
        });

        return services;
    }


    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Database connection string 'DefaultConnection' not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)); // Get from config


        services.AddScoped<ApplicationDbSeeder>(); // seed data / ensure migrations applied inside


        return services;

    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            //options.Password.RequireDigit = true;
            //options.Password.RequiredLength = 8;
            //options.Password.RequireLowercase = true;
            //options.Password.RequireUppercase = true;
            //options.Password.RequireNonAlphanumeric = false;

            //options.SignIn.RequireConfirmedAccount = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JwtOptionsKey));

        services.AddScoped<AuthTokenService>();
        services.AddScoped<AccountService>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddCookie().AddGoogle(options =>
        {
            // add to secrets later.
            var googleConfig = configuration.GetSection("Authentication:Google");
            var clientId = googleConfig["ClientId"];
            var clientSecret = googleConfig["ClientSecret"];

            ArgumentNullException.ThrowIfNull(clientId, nameof(clientId));
            ArgumentNullException.ThrowIfNull(clientSecret, nameof(clientSecret));


            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionsKey)
                .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

            if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey) || jwtOptions.SecretKey.Length < 32)
            {
                throw new InvalidOperationException("JWT secret key is too short. It must be at least 32 characters.");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
            };

            // attach as cookie
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies[CookieHelper.AccessTokenName];
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }


    public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ApplicationPolicy.BasicPolicy, policy => policy.RequireRole(ApplicationRole.BasicAccessRoles));
            options.AddPolicy(ApplicationPolicy.PremiumPolicy, policy => policy.RequireRole(ApplicationRole.PremiumAccessRoles));
            options.AddPolicy(ApplicationPolicy.AdminPolicy, policy => policy.RequireRole(ApplicationRole.Admin));


            //// You can add more complex policies as needed
            //options.AddPolicy("RequireConfirmedAccount", policy =>
            //    policy.RequireAssertion(context =>
            //        context.User.HasClaim(c =>
            //            c.Type == "EmailConfirmed" && c.Value == "true")));

            //// Default policy requirements
            //options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();

            //// Fall back policy when no policy is specified
            //options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
        });

        return services;
    }


    public static IServiceCollection ConfigureFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints();

        return services;
    }


    public static IServiceCollection ConfigureSPACors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowNuxtFrontend",
               policyBuilder =>
               {
                   policyBuilder
                   .WithOrigins("https://localhost:3000", "https://accounts.google.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
               });

        });

        return services;
    }



};

