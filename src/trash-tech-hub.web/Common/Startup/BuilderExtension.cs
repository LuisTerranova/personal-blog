using TrashTechHub.Application.Services;
using TrashTechHub.Core.Services;
using TrashTechHub.Infrastructure;

namespace TrashTechHub.Web.Common.Startup;

public static class BuilderExtension
{
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => 
            {
                policy.RequireRole("admin"); 
            });
        });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TrashTechHub.Application.Mappings.MappingProfile>());
    }


    public static void AddDataContext(this WebApplicationBuilder builder)
    {
        var connStr = !builder.Environment.IsEnvironment("Testing")
            ? builder.Configuration.GetConnectionString("DefaultConnection")
            : null;

        builder.Services.AddInfrastructure(connStr);

        builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/admin/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });
    }
}
