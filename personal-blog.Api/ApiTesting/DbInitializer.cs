using Microsoft.AspNetCore.Identity;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.ApiTesting;

public static class DbInitializer
{
    public static async Task SeedAdminUserAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<long>>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            const string adminRole = "admin";
            
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole<long>(adminRole));
            }
            
            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];
            
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during DB seeding: {ex.Message}");
        }
    }
}