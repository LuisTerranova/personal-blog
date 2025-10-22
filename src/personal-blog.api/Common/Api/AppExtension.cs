using Microsoft.EntityFrameworkCore;
using personal_blog.Api.ApiTesting;
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
                
                logger.LogInformation("A aplicar migrações da base de dados...");
                var context = services.GetRequiredService<AppDbContext>();
                
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrações aplicadas com sucesso.");
                logger.LogInformation("A executar o seeding do utilizador Admin...");
                
                await app.SeedAdminUserAsync(); //Mock data for testing
                logger.LogInformation("Seeding concluído.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocorreu um erro durante a migração ou seeding.");
            }
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}