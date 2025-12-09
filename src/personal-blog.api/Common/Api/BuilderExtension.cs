using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.Api.Handlers;
using personal_blog.Api.Models;
using personal_blog.core;
using personal_blog.core.Handlers;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace personal_blog.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString =
            builder
                .Configuration
                .GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });
    }

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
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<IPostHandler, PostHandler>();
        builder.Services.AddTransient<IProjectHandler, ProjectHandler>();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("BlazorApp", policy =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    policy.WithOrigins(
                            "http://localhost:5096",
                            "https://localhost:5096",
                            "http://localhost:5164",
                            "https://localhost:5164",
                            //Docker
                            "http://localhost:8080",  
                            "http://localhost:8082"   
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
                else
                {
                    var origins = builder.Configuration.GetValue<string>("CORS_ORIGINS");
                    if (!string.IsNullOrEmpty(origins))
                    {
                        policy.WithOrigins(origins.Split(','))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); 
                    }
                }
            });
        });
    }


    public static void AddDataContext(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsEnvironment("Testing"))
        {
            return;
        }
        builder.Services
            .AddDbContext<AppDbContext>
                (x => { x.UseNpgsql(Configuration.ConnectionString); })  
            .AddIdentity<ApplicationUser, IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Impede o redirecionamento para /Account/Login quando não autenticado
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            // Impede o redirecionamento para /Account/AccessDenied quando sem permissão
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });
    }
}