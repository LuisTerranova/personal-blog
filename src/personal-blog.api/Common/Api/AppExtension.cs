using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Common.MockData;
using personal_blog.Api.Data;

namespace personal_blog.Api.Common.Api;

public static class AppExtension
{
    public static async Task ConfigureDevEnvironment(this WebApplication app)
    {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Applying data migrations...");
                var context = services.GetRequiredService<AppDbContext>();
                
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
                logger.LogInformation("Executing admin seed...");
                
                await app.SeedAdminUserAsync(); //Mock data for testing
                logger.LogInformation("Seeding concluded successfully.");
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