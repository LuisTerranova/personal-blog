using Microsoft.EntityFrameworkCore;
using TrashTechHub.Core.Models;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Web.Common.MockData;

public static class DbInitializer
{
    public static async Task SeedAdminUserAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        if (await context.AdminUsers.AnyAsync())
        {
            logger.LogInformation("Admin user already exists, skipping seed.");
            return;
        }

        var email = config["AdminSettings:Email"] ?? "admin@trashtechhub.com";
        var password = config["AdminSettings:Password"] ?? "admin";

        context.AdminUsers.Add(new AdminUser
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        });
        await context.SaveChangesAsync();

        logger.LogInformation("Admin user seeded: {Email}", email);
    }
}
