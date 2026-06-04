using Microsoft.EntityFrameworkCore;
using TrashTechHub.Web.Common.MockData;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Web.Common.Startup;

public static class AppExtension
{
    public static async Task ConfigureEnvironment(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Applying data migrations...");
            var context = services.GetRequiredService<AppDbContext>();
            
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }
            logger.LogInformation("Migrations applied successfully.");

            await app.SeedAdminUserAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}