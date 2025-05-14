// Infrastructure/ApplicationBuilderExtensions.cs
using Backend.Core;
using Backend.Infrastructure.Identity.Constants;
using FastEndpoints;
using Scalar.AspNetCore;

namespace Backend.Startup;
public static class ApplicationBuilderExtensions
{

    public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.MapOpenApi();
            app.MapScalarApiReference(opt =>
            {
                opt.WithTitle("JWT + Refresh Token Auth API");
                opt.Theme = ScalarTheme.Saturn;
                opt.Layout = ScalarLayout.Modern;
                opt.DarkMode = false;
                opt.HideClientButton = true;
            });
        }
        else
        {
            app.UseHsts();
        }

        app.UseExceptionHandler(_ => { });
        app.UseHttpsRedirection();

        app.UseCors("AllowNuxtFrontend");

        // Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseFastEndpoints(x => x.Errors.UseProblemDetails());

        return app;
    }
    public static WebApplication AddSampleRoleEndpoints(this WebApplication app)
    {
        app.MapGet(pattern: "/api/public",
         () => ApiResponse<string>.Success("Everyone can see this."));


        app.MapGet(pattern: "/api/basic",
            () => ApiResponse<string>.Success("Thanks for being a member."))
                    .RequireAuthorization();

        app.MapGet(pattern: "/api/premium",
            () => ApiResponse<string>.Success("Thanks for being a premium member."))
                    .RequireAuthorization(ApplicationPolicy.PremiumPolicy);

        app.MapGet(pattern: "/api/admin",
           () => ApiResponse<string>.Success("Super secret company data"))
                   .RequireAuthorization(ApplicationPolicy.AdminPolicy);



        return app;
    }
}
