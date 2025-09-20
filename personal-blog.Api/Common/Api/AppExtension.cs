using personal_blog.Api.ApiTesting;

namespace personal_blog.Api.Common.Api;

public static class AppExtension
{
    public static async void ConfigureDevEnvironment(this WebApplication app)
    {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            await app.SeedAdminUserAsync(); //Mock data for testing
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}